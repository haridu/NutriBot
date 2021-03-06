﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Nutribot.models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather_Bot.Models
{
    [LuisModel("fa67b7f5-e95d-485b-91ac-01aec2d1956d", "70a969c682e94fb0a047edde403fe126")]
    [Serializable]
    public class Luis : LuisDialog<Object>
    {
        String username = "";
        String foodname = "";
        String id = "";

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            _message = (Activity)await item;
            await base.MessageReceived(context, item);



        }


        [field: NonSerialized()]
        private Activity _message;




        String endOutput = "";
        private string user;

        public Luis(string user)
        {
            this.user = user;
        }

        [LuisIntent("Viewfavorites")]
        public async Task viewlist(IDialogContext context, LuisResult result)
        {



            List<Timeline> timelines = await AzureManager.AzureManagerInstance.GetTimelines();
            await context.PostAsync("showing list");
            endOutput = "";

            int item = 0;
            var markdownContent = this.user+" #Favorites List\n";
            foreach (Timeline t in timelines)
            {
                if ((!t.username.Equals(null)) || (!(t.username.Equals(null)))){
                    if (t.username.Equals(this.user))
                    {
                        item++;
                        markdownContent += "** item id[" + t.id + "] Food name =" + t.food + "** importance = *" + t.importance + "* created in =" + t.createdAt + "\n\n";
                    }
                }
            }
            markdownContent += "![](https://cloud.githubusercontent.com/assets/7879247/20590892/90b146e8-b28a-11e6-855e-1eb9beb7f7b4.jpg)\n";
            markdownContent += "```\n" + item + " items in Favorite list \n```\n";

            Activity reply = _message.CreateReply(markdownContent);

            await context.PostAsync(reply);



            // await stateClient.BotState.SetUserDataAsync(_message.ChannelId, _message.From.Id, userData);
            context.Wait(MessageReceived);

        }




        [LuisIntent("additem")]
        public async Task addtolist(IDialogContext context, LuisResult result)
        {
            int id = 0;
            List<Timeline> timelines = await AzureManager.AzureManagerInstance.GetTimelines();
            foreach (Timeline t in timelines) {
                id++;
            }

            id = id + 1;

                if (!foodname.Equals(""))
            {
                Timeline timeline = new Timeline()
                {
                    id=id.ToString(),
                    username = this.user,
                    food = foodname,
                    importance = "normal",
                    createdAt = DateTime.Now
                };

                await AzureManager.AzureManagerInstance.AddTimeline(timeline);

                var markdownContent = "#" + foodname + "added to favorites  \n";
                markdownContent += foodname + "added to favorites  at[" + timeline.createdAt + "]with normal importance\n\n";
                markdownContent += "![](https://cloud.githubusercontent.com/assets/7879247/20590892/90b146e8-b28a-11e6-855e-1eb9beb7f7b4.jpg)\n";
                Activity areply = _message.CreateReply(markdownContent);

                await context.PostAsync(areply);
                //endOutput = foodname + "added to favorites  at[" + timeline.createdAt + "]with normal importance";
            }
            else
            {
                // endOutput = "no food item is selected by searching foods";

                var markdownContent = "#no food item is selected\n";
                markdownContent += "no food item is selected by searching foods\n\n";
                markdownContent += "![](https://cloud.githubusercontent.com/assets/7879247/20590890/8e8eb396-b28a-11e6-8ab1-89e0310c9647.jpg)\n";
                Activity areply = _message.CreateReply(markdownContent);
                await context.PostAsync(areply);
            }
            //await context.PostAsync(endOutput);
            //context.Wait(MessageReceived);

        }


       



        [LuisIntent("getnutrition")]
        public async Task getitem(IDialogContext context, LuisResult result)
        {

            String itemname = "";
            EntityRecommendation recomendation;

            if (result.TryFindEntity("food", out recomendation))
            {

                itemname = recomendation.Entity;
                await context.PostAsync("item is " + itemname);

                Nutriobject.RootObject rootObject;



                HttpClient client = new HttpClient();

                if (recomendation.Entity == null)
                {
                    await context.PostAsync("not found" + itemname);
                }

                //commented becouse need to save api calls
                string x = await client.GetStringAsync(new Uri("https://api.nutritionix.com/v1_1/search/" + itemname + "?results=0%3A1&cal_min=0&cal_max=50000&fields=item_name%2Cbrand_name%2Citem_type%2Cnf_total_fat%2Cnf_calories%2Cnf_total_carbohydrate%2Cnf_dietary_fiber%2Cnf_sugars%2Cnf_protein%2Cnf_vitamin_c_dv%2Cnf_calcium_dv%2Cnf_iron_dv&appId=f675b8ac&appKey=f9f46ce8fd9a7584e439b145bcfb4386"));
                if (x == null || x.Equals(" "))
                {
                    await context.PostAsync("not found" + itemname);
                }
                rootObject = JsonConvert.DeserializeObject<Nutriobject.RootObject>(x);

                int cityName = rootObject.total_hits;
                //string temp = rootObject.brand_name;
                foreach (Nutriobject.Hit hits in rootObject.hits)
                {
                    foodname = hits.fields.item_name;
                    //String description = hits.fields.item_description.ToString;
                    String brandname = hits.fields.brand_name;

                    double calories = hits.fields.nf_calories;
                    double totalcobohydrates = hits.fields.nf_total_carbohydrate;
                    double total_fat = hits.fields.nf_total_fat;
                    double sugers = hits.fields.nf_sugars;
                    double calciaum = hits.fields.nf_calcium_dv;
                    double iron = hits.fields.nf_iron_dv;
                    String servingsizeunit = hits.fields.nf_serving_size_unit;




                    string xi = await client.GetStringAsync(new Uri("https://api.nutritionix.com/v1_1/search/" + itemname + "?results=0%3A1&cal_min=0&cal_max=50000&fields=item_name%2Cbrand_name%2Citem_type%2Cnf_total_fat%2Cnf_calories%2Cnf_total_carbohydrate%2Cnf_dietary_fiber%2Cnf_sugars%2Cnf_protein%2Cnf_vitamin_c_dv%2Cnf_calcium_dv%2Cnf_iron_dv&appId=f675b8ac&appKey=f9f46ce8fd9a7584e439b145bcfb4386"));
                    Activity weatherReply = _message.CreateReply($"");
                    weatherReply.Recipient = _message.From;
                    weatherReply.Type = "message";
                    weatherReply.Attachments = new List<Attachment>();


                    var markdownContent = "# " + brandname + "'s " + foodname + " Nutrition Facts\n";
                    markdownContent += "Calories =" + calories + " kcal\n\n";
                    markdownContent += "sugers =" + sugers + " g\n\n";
                    markdownContent += "calciam =" + calciaum +" %\n\n";
                    markdownContent += " iron =" + iron + " %\n\n";

                    markdownContent += "![](https://cloud.githubusercontent.com/assets/7879247/20590871/82ece74c-b28a-11e6-9fe6-18a0ab7c5c63.jpg)\n";

                    Activity reply = _message.CreateReply(markdownContent);

                    await context.PostAsync(reply);
                }

            }
            else
            {
                await context.PostAsync("not sure about" + itemname);

            }

            context.Wait(MessageReceived);
        }




        [LuisIntent("")]
        public async Task none(IDialogContext context, LuisResult result)
        {
            var markdownContent = "# unknown command  \n";
            markdownContent += "sorry,i dont have any idea what you are talking about\n\n";
            markdownContent += "![](https://cloud.githubusercontent.com/assets/7879247/20590890/8e8eb396-b28a-11e6-8ab1-89e0310c9647.jpg)\n";
            Activity areply = _message.CreateReply(markdownContent);
            await context.PostAsync(areply);
            // await context.PostAsync("i dont have any idea what you are talking about");
            context.Wait(MessageReceived);
        }

    }
}