using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSocketSharp;
using SplytPartnerSDK.Model;
using SplytPartnerSDK.Exceptions;
using SplytPartnerSDK.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SplytPartnerSDK
{
    public class Client
    {
        private WebSocket wss;
        public bool DEBUG { get; set; } = false;

        private readonly string url;
        private readonly string login;
        private readonly string password;

        private readonly List<Message> messageQueue;
        private bool authenticated = false;
        private bool connected = false;

        private readonly Dictionary<string, Action<OnPushArgs>> events;
        private readonly Dictionary<string, TaskCompletionSource<JObject>> callbacks;

        private const string METHOD_SIGNIN = "partner.sign-in";
        private const int REQUEST_TIMEOUT = 30000;
        private const int PING_INTERVAL = 5000;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:partnersdk.SplytPartnerSDK.Client"/> class.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="login">Partner login</param>
        /// <param name="password">Partner password</param>
        public Client(string url, string login, string password) {
            this.url = url;
            this.login = login;
            this.password = password;

            messageQueue = new List<Message>();
            events = new Dictionary<string, Action<OnPushArgs>>();
            callbacks = new Dictionary<string, TaskCompletionSource<JObject>>();

            var keepAlive = CheckConnectionAlive();

            //NetworkChange.NetworkAvailabilityChanged += async (sender, args) => {
            //	if (args.IsAvailable) {
            //                 Console.WriteLine("Network is now available, reconnecting...");
            //                 await Disconnect();
            //                 await Connect();
            //	}
            //};
        }

        /// <summary>
        /// Connect to WSAPI
        /// </summary>
        /// <returns>The start.</returns>
        private async Task<bool> Connect() {
            if (DEBUG) Console.WriteLine("Connecting...");

            wss = new WebSocket(url, onOpen: OnOpen, onClose: OnClose,
                                onError: OnError, onMessage: OnMessage,
                                protocols: new string[] { "splyt-protocol" });

            authenticated = false;
            connected = false;

            // - Try to connect and authenticate on connection
            if (await wss.Connect()) {
                connected = true;
                if (DEBUG) Console.WriteLine("Connected, trying to authenticate.");
                authenticated = await SignIn();

                if(authenticated) {
                    Console.WriteLine("Connection authenticated");
                    ProcessQueue();
                } else {
                    Console.WriteLine("Connection failed to authenticate");
                    await Disconnect();
                }

				return true;
            } else {
                if (DEBUG) Console.WriteLine("Failed to connect.");
                await Disconnect();
            }

            return false;
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <returns>The in.</returns>
        private async Task<bool> SignIn() {
            try {
				await Request(METHOD_SIGNIN, new {
					login,
					password,
				});
                return true;
            } catch(Exception) {
                return false;
            }
		}

        /// <summary>
        /// Disconnect this instance.
        /// </summary>
        /// <returns>The disconnect.</returns>
        private async Task Disconnect() {
            await wss.Close();
            wss.Dispose();
            wss = null;
            connected = false;
        }

        /// <summary>
        /// Reconnect this instance.
        /// </summary>
        private async Task Reconnect() {
            await Disconnect();
            await Task.Delay(2000);
            await Connect();
        }

        /// <summary>
        /// Checks the connection alive.
        /// </summary>
        /// <returns>The connection alive.</returns>
		private async Task CheckConnectionAlive() {
            while (true) {
                if (DEBUG) Console.WriteLine("checking connection");

                if (connected) {
                    try {                        
                        await Request("system.echo", new { ping = "pong" }, PING_INTERVAL);
                        if (DEBUG) Console.WriteLine("Alive");
                    } catch(TimeoutException ex) {
						if (DEBUG) Console.WriteLine("Dead" + ex);
						Task reconnect = Reconnect();
                    }
                } else if (wss == null) {
                    Task connect = Connect();
                }

                await Task.Delay(PING_INTERVAL);
            }
        }

        /// <summary>
        /// Ons the open.
        /// </summary>
        /// <returns>The open.</returns>
        private async Task OnOpen() {
            if (DEBUG) { Console.WriteLine("OnOpen"); }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Ons the close.
        /// </summary>
        /// <returns>The close.</returns>
        private async Task OnClose(CloseEventArgs args) {
            if (DEBUG) { Console.WriteLine("OnClose - clean? " + args.WasClean); }
            await Disconnect();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Ons the message.
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="args">The ${ParameterType} instance containing the event data.</param>
        private async Task OnMessage(MessageEventArgs args) {
            var text = await args.Text.ReadToEndAsync();
            if (DEBUG) { Console.WriteLine($"OnMessage [{text}]"); }
            Message message = JsonConvert.DeserializeObject<Message>(text, new MessageConverter());
            var data = message.HasData() ? JObject.FromObject(message.Data) : new JObject();

            if (message is Response response) {
                // - Handle response
                TaskCompletionSource<JObject> callback;

                if (callbacks.TryGetValue(response.Id, out callback)) {
                    if (response.Success) {
                        callback.TrySetResult(data);
                    } else {
                        var responseError = data.ToObject<ResponseError>();
                        callback.TrySetException(new ResponseErrorException(responseError));
                    }
                }
            } else if (message.IsPush()) {
                // - Handle push
                Action<OnPushArgs> action;
                var pushArgs = new OnPushArgs(message.Method, data);
                if (events.TryGetValue(message.Method, out action)) {
                    action(pushArgs);
                } else if(events.TryGetValue("*", out action)) {
                    action(pushArgs);
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Ons the error.
        /// </summary>
        /// <returns>The error.</returns>
        private async Task OnError(ErrorEventArgs args) {
            if (DEBUG) { Console.WriteLine($"OnError [{args.Message}]"); }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Send the specified request.
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="message">Message.</param>
        private async Task<bool> Send(Message message) {
            var text = JsonConvert.SerializeObject(message);
            var result = await wss.Send(text);
            if (DEBUG) { Console.WriteLine($"Sent text? [{result}] -> [{text}]"); }
            return result;
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        private async void ProcessQueue() {
            if (wss == null || !connected) return;
            foreach (Message message in new List<Message>(messageQueue)) {
                if (authenticated || METHOD_SIGNIN.Equals(message.Method)) {
                    await Send(message);
                    messageQueue.Remove(message);
                }
            }
        }

        /// <summary>
        /// Request the specified method and data.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="method">Method.</param>
        /// <param name="data">Data.</param>
        public async Task<JObject> Request(String method, Object data, int timeout = REQUEST_TIMEOUT) {
            var request = Message.Request(method, data);
            var source = new TaskCompletionSource<JObject>();

            callbacks.Add(request.Id, source);
            messageQueue.Add(request);
            ProcessQueue();

            var completed = await Task.WhenAny(source.Task, Task.Delay(timeout));

            if (completed != source.Task) {
                messageQueue.Remove(request);
                if (DEBUG) { Console.WriteLine($"Request timed out [{request.Id}]"); }
                source.TrySetException(new TimeoutException());
            }

            return await source.Task;
        }

        /// <summary>
        /// Register handler for push message with given method
        /// </summary>
        /// <returns>The on.</returns>
        /// <param name="method">Method.</param>
        public void On(String method, Action<OnPushArgs> action) {
            events.Add(method, action);
        }

        /// <summary>
        /// Register fallback handler for unhandled push messages
        /// </summary>
        /// <returns>The on.</returns>
        /// <param name="action">Action.</param>
        public void On(Action<OnPushArgs> action) {
            On("*", action);
        }
    }
}
