using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Models
{
    public class CustomMessages
    {
        public string text { get; set; }
        public List<Button> buttons { get; set; }
        public List<Card> cards { get; set; }


        public class Button
        {
            public string title { get; set; }
            public string value { get; set; }
            public string type { get; set; }

        }
        public class Card
        {
            public string title { get; set; }
            public string text { get; set; }
            public string image { get; set; }
            public List<Button> buttons { get; set; }
        }

        public IMessageActivity getActivityFromMessage(IDialogContext context)
        {
            IMessageActivity tmp = context.MakeMessage();
            if (!string.IsNullOrEmpty(text)) tmp.Text = text;
            if(buttons != null && buttons.Count > 0)
            {
                List<CardAction> list = new List<CardAction>();
                foreach(Button b in buttons)
                {
                    CardAction c = new CardAction();
                    c.Text = b.title;
                    c.Title = b.title;
                    c.Value = string.IsNullOrWhiteSpace(b.value) ? c.Text : b.value;
                    c.Type = string.IsNullOrWhiteSpace(b.type) ? "postBack" : b.type;
                    list.Add(c);
                }
                HeroCard h = new HeroCard();
                h.Buttons = list;
                h.Text = text;
                tmp.Text = "";
                tmp.Attachments = new List<Attachment>() { h.ToAttachment()};
            }
            if(cards != null && cards.Count() > 0)
            {
                tmp.Attachments = new List<Attachment>();
                List<HeroCard> listcard = new List<HeroCard>();
                foreach(Card card in cards)
                {
                    HeroCard tmpcard = new HeroCard();
                    List<CardAction> list = new List<CardAction>();
                    foreach (Button b in card.buttons)
                    {
                        CardAction c = new CardAction();
                        c.Text = b.title;
                        c.Title = b.title;
                        c.Value = string.IsNullOrWhiteSpace(b.value) ? c.Text : b.value;
                        c.Type = string.IsNullOrWhiteSpace(b.type) ? "postBack" : b.type;
                        list.Add(c);
                    }
                    tmpcard.Buttons = list;
                    tmpcard.Text = card.text;
                    tmpcard.Title = card.title;
                    listcard.Add(tmpcard);
                    tmp.Attachments.Add(tmpcard.ToAttachment());
                }
            }
            return tmp;
        }

    }
}