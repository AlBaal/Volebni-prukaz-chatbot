﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using VolebniPrukaz.API.Facebook;
using VolebniPrukaz.Dialogs;

namespace VolebniPrukaz
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        ///     POST: api/Messages
        ///     Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                FacebookClient fbClient = new FacebookClient();
                fbClient.SendTyping(activity.From.Id);
            }).Start();

            if (activity.Type == ActivityTypes.Message)
            {
                //Reset conversation on facebook start
                if(activity.Text == "STARTED_CON")
                    activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);

                await Conversation.SendAsync(activity, () => RootDialog.StartWithHelloChain());
            }
            else
                await HandleSystemMessage(activity);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
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
        }
    }
}