using System;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Managers;
using SmaugCS.Common;

namespace SmaugCS.Objects
{
    public class NoteData
    {
        public string Sender { get; set; }
        public string DateSent { get; set; }
        public string RecipientList { get; set; }
        public string Subject { get; set; }
        public int Voting { get; set; }
        public string YesVotes { get; set; }
        public string NoVotes { get; set; }
        public string Abstentions { get; set; }
        public string Text { get; set; }

        public void Load(TextSection section)
        {
            bool inTextSection = false;

            foreach (string line in section.Lines)
            {
                if (inTextSection)
                {
                    Text += line;
                    continue;
                }

                string firstWord = line.FirstWord();
                string remainder = line.RemoveWord(1).TrimEnd('~');

                switch (firstWord.ToUpper())
                {
                    case "SENDER":
                        Sender = remainder;
                        break;
                    case "DATE":
                        DateSent = remainder;
                        break;
                    case "TO":
                        RecipientList = remainder;
                        break;
                    case "SUBJECT":
                        Subject = remainder;
                        break;
                    case "VOTING":
                        Voting = remainder.ToInt32();
                        break;
                    case "YESVOTES":
                        YesVotes = remainder;
                        break;
                    case "NOVOTES":
                        NoVotes = remainder;
                        break;
                    case "ABSTENTIONS":
                        Abstentions = remainder;
                        break;
                    case "TEXT":
                        inTextSection = true;
                        break;
                    default:
                        LogManager.Bug("Unknown parameter {0}", firstWord);
                        break;
                }
            }
        }
    }
}
