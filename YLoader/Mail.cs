using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace YLoader
{
    class EmailLetter
    {
        String addressatMaii = "";
        String titleOfLetter = "";
        String letterText = "";

        public EmailLetter(String addressatMaii_, String titleOfLetter_, String letterText_)
        {
            addressatMaii = addressatMaii_;
            titleOfLetter = titleOfLetter_;
            letterText = letterText_;
        }
        /// <summary>
        /// Send letter from vadykakonon@gmail.com
        /// </summary>
        /// <param name="addresat">That guy, who take a letter</param>
        /// <param name="titleOfLetter"></param>
        /// <param name="letterText"></param>
        public void send()
        {
            SmtpClient C = new SmtpClient("smtp.gmail.com");
            C.Port = 587;
            C.Credentials = new NetworkCredential("vadykakonon", "pitqhauddgybnhzz");
            C.EnableSsl = true;
            C.Send(new MailMessage("vadykakonon@gmail.com", addressatMaii, titleOfLetter, letterText));
            // "SYNTAX: mail-1 <ToAddr> <Title>"
        }

    }
    class MailStaff
    {
        List<EmailLetter> emailLetters = new List<EmailLetter>();
        public int Count() { return emailLetters.Count; }
        public void readLetters(string pathToFile) //get letters from file
        {
            //Buffer
            String addressatMaii = "";
            String titleOfLetter = "";
            String letterText = "";
            //Method
            File.ReadAllLines(pathToFile).ToList().ForEach(line =>
            { //for each line
                List<String> lineList = line.Split(':').ToList().Select(x => x.Trim()).ToList(); //get info
                switch (lineList[0])
                {
                    case "Addressat":
                        addressatMaii = lineList[1];
                        break;
                    case "Title":
                        titleOfLetter = lineList[1];
                        break;
                    case "[LETTER END]": //if there are letter - letter is addedd
                        if (addressatMaii != "" && titleOfLetter != "" && letterText != "")
                            emailLetters.Add(new EmailLetter(addressatMaii, titleOfLetter, letterText));
                        addressatMaii = ""; titleOfLetter = ""; letterText = "";
                        break;
                    case "":
                        break;
                    default:
                        letterText += line + "\r\n";
                        break;
                }
            });
        }
        public void SendAll()
        {
            emailLetters.ForEach(x => x.send());
        }

        public void SaveExample(String path, String fileName = "example")
        {
            path += $"\\{fileName}.txt";
            String message = "";
            message += "Addressat: oao@gmail.com\r\nTitle: Hey\r\n...\r\n...\r\n...\r\n..\r\n[LETTER END]\r\n\r\n";
            message += "Addressat: oaoao@gmail.com\r\nTitle: Hey\r\n...\r\n...\r\n...\r\n..\r\n[LETTER END]\r\n\r\n";
            File.WriteAllText(path, message);
        }
    }
}
