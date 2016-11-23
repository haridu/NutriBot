using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

using System.Collections.Generic;
using Nutri_Bot;

using Microsoft.Bot.Builder.Dialogs;
using Weather_Bot.Models;


namespace Nutri_Bot
{


    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {



                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                var userMessage = activity.Text;




                //String endOutput = "";

                switch (userMessage.ToLower())
                {
                    case "clear":
                        await stateClient.BotState.DeleteStateForUserAsync(activity.ChannelId, activity.From.Id);
                        Activity reply = activity.CreateReply($"User data is cleared");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;
                    case "hello":
                    case "hey":
                    case "hi":
                        if (userData.GetProperty<bool>("SentGreeting"))
                        {
                            Activity helloagain = activity.CreateReply($"hello again " + userData.GetProperty<string>("Name") + " :)");
                            await connector.Conversations.ReplyToActivityAsync(helloagain);
                        }
                        else
                        {
                            Activity hello = activity.CreateReply($"Hi! ,im Nutrio bot i can get nutrition information of food and save your food into favorites list as you wish,to use this app you have set your name first :)");
                            await connector.Conversations.ReplyToActivityAsync(hello);
                            userData.SetProperty<bool>("SentGreeting", true);
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        }
                        break;
                    default:


                        if (userMessage.Length < 8)
                        {
                            Activity passedhere = activity.CreateReply($"unknowen command");
                            await connector.Conversations.ReplyToActivityAsync(passedhere);
                            break;
                        }


                        if (userMessage.ToLower().Substring(0, 8).Equals("set name"))
                        {
                            string name = userMessage.Substring(9);
                            userData.SetProperty<string>("Name", name);
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                            Activity nameassined = activity.CreateReply($"name is assigned " + userData.GetProperty<string>("Name") + " , you can continue now :)");
                            await connector.Conversations.ReplyToActivityAsync(nameassined);

                        }
                        else
                        {
                            string name = userData.GetProperty<string>("Name");
                            if (name == null)
                            {
                                //endOutput = "Home City not assigned";
                                Activity names = activity.CreateReply($"  name is not assigned ,you have to set your to use this app,assign name by typing set name <yourname>");
                                await connector.Conversations.ReplyToActivityAsync(names);

                            }
                            else
                            {

                                await Conversation.SendAsync(activity, () => new Luis());
                            }

                        }
                        break;


                }

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}