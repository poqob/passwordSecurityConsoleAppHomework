/****************************************************************************
**					SAKARYA ÜNİVERSİTESİ
**				BİLGİSAYAR VE BİLİŞİM BİLİMLERİ FAKÜLTESİ
**				    BİLGİSAYAR MÜHENDİSLİĞİ BÖLÜMÜ
**				   NESNEYE DAYALI PROGRAMLAMA DERSİ
**					2014-2015 BAHAR DÖNEMİ
**	
**				ÖDEV NUMARASI.........:  1
**				ÖĞRENCİ ADI............: Poqob
**				ÖĞRENCİ NUMARASI.......: zumzum
**                         DERSİN ALINDIĞI GRUP...: C
****************************************************************************/

using System;
using study1;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace study1
{
    class Program
    {
        static public void Main(string[] args)
        {

            Password password = new Password();
            ConsoleRenderer.screen(0, ref password.passSecurityScore, ref password.pass);
            ControllClass.takeInputAndShowOnConsole(password);
        }
    }
    //Password Class
    partial class Password
    {
        public int passSecurityScore = 0;
        short howManyUpper = 0;
        short howManyLower = 0;
        short howManyNumber = 0;
        short howManySymbol = 0;
        short count = 0;

        public List<char> pass = new List<char>();
        //if isPasswordAcceptable array's every item is true then this password is allowed to use.
        public bool[] isPasswordAcceptable = { false, false, false, false, false };


        //pass secure controll
        public void passSecure()
        {
            passSecurityScore = 0;
            amILongerThanNineCharsAndAcceptable(ref pass);
            doIHaveLowerLetter(ref pass);
            doIHaveUpperLetter(ref pass);
            doIHaveNumber(ref pass);
            doIHaveSymbol(ref pass);
            scoring(ref pass);
        }

        void amILongerThanNineCharsAndAcceptable(ref List<char> pass)
        {
            if (pass.Count > 8)
            {
                isPasswordAcceptable[0] = true;
                passSecurityScore += 10;
            }
            else
            {
                isPasswordAcceptable[0] = false;
            }



        }
        void doIHaveSymbol(ref List<char> pass)
        {
            foreach (char item in pass)
            {
                if (new Regex("^['!#$%&'*+/=?^_`{|}~.-]*$").IsMatch(Convert.ToString(pass[count])))
                {
                    howManySymbol++;
                }
                count++;
            }
            passSecurityScore += (howManySymbol * 10);
            if (howManySymbol > 1) { isPasswordAcceptable[1] = true; }
            count = 0;
            howManySymbol = 0;
        }

        void doIHaveNumber(ref List<char> pass)
        {
            foreach (char item in pass)
            {
                if (new Regex(@"^\d$").IsMatch(Convert.ToString(pass[count])))
                {
                    howManyNumber++;
                }
                count++;
            }
            if (howManyNumber > 1) { passSecurityScore += 20; isPasswordAcceptable[2] = true; } else { passSecurityScore += howManyNumber * 10; isPasswordAcceptable[2] = false; }
            count = 0;
            howManyNumber = 0;
        }

        void doIHaveUpperLetter(ref List<char> pass)
        {
            foreach (char item in pass)
            {
                if (new Regex(@"[A-Z]").IsMatch(Convert.ToString(pass[count])))
                {
                    howManyUpper++;
                }
                count++;
            }
            if (howManyUpper > 2) { passSecurityScore += 20; } else { passSecurityScore += howManyUpper * 10; isPasswordAcceptable[3] = false; }
            if (howManyUpper > 1) { isPasswordAcceptable[3] = true; }
            count = 0;
            howManyUpper = 0;
        }

        void doIHaveLowerLetter(ref List<char> pass)
        {
            foreach (char item in pass)
            {
                if (new Regex(@"[a-z]").IsMatch(Convert.ToString(pass[count])))
                {
                    howManyLower++;
                }
                count++;
            }
            if (howManyLower > 2) { passSecurityScore += 20; } else { passSecurityScore += howManyLower * 10; isPasswordAcceptable[4] = false; }
            if (howManyLower > 1) { isPasswordAcceptable[4] = true; }
            count = 0;
            howManyLower = 0;
        }

        void scoring(ref List<char> pass)
        {
            if (pass.Count > -1 && pass.Count < 5)
            {
                ConsoleRenderer.passwordStrenghtMessage = "very weak";
            }
            else if (pass.Count < 8 && pass.Count > 4)
            {
                ConsoleRenderer.passwordStrenghtMessage = "weak";
            }
            else if (passSecurityScore > 60)
            {
                ConsoleRenderer.passwordStrenghtMessage = "acceptable";
            }
        }
    }

    static class ControllClass
    {
        static public void takeInputAndShowOnConsole(Password password)
        {
            //to controll which key pressed on keyboard.
            ConsoleKey key;
            //to convert and attemt the ConsoleKey to char.
            char currrentInput;
            do
            {
                //converting ConsoleKey variant to char.
                currrentInput = Console.ReadKey(true).KeyChar;
                //attemt current input's value to key as ConsoleKey type.
                key = ((ConsoleKey)currrentInput);

                generalControll(ref password, key, ref currrentInput);
                //printing to screen the output.
                ConsoleRenderer.screen(password.pass.Count, ref password.passSecurityScore, ref password.pass);

            } while (key != ConsoleKey.Escape);
        }
        static public void generalControll(ref Password password, ConsoleKey key, ref char currrentInput)
        {
            //if user press backSpace key, this function will trigger and delete last item of password.
            //if user press spaceBar, this function will send exception to console.
            isPasswordAcceptableFunction(key, ref password);
            controllingBackSpaceAndSpaceBar(key, ref password);
            if (isMyInputSuitsMyCharacterCollections(ref currrentInput))
            {
                allowToAdd(ref currrentInput, ref password);
                password.passSecure();
            }
        }
        static private void isPasswordAcceptableFunction(ConsoleKey key, ref Password password)
        {
            char controll = 'a';
            if (key == ConsoleKey.Enter)
            {

                foreach (bool item in password.isPasswordAcceptable)
                {
                    if (item == false)
                    {
                        controll = 'b';
                    }
                }
                if (controll != 'a')
                {
                    ConsoleRenderer.lastExeptionCode = 2;
                }
                else { ConsoleRenderer.lastExeptionCode = 3; }
            }
        }
        static private void controllingBackSpaceAndSpaceBar(ConsoleKey key, ref Password password)
        {
            if (key == ConsoleKey.Spacebar)
            {
                ConsoleRenderer.lastExeptionCode = 1;
            }
            else if (key == ConsoleKey.Backspace && password.pass.Any())
            {
                password.pass.RemoveAt(password.pass.Count - 1);
                password.passSecure();
            }
        }

        static private bool isMyInputSuitsMyCharacterCollections(ref char input)
        {
            return new Regex("^[a-zA-Z0-9'!#$%&'*+/=?^_`{|}~.-]*$").IsMatch(Convert.ToString(input));
        }

        static private void allowToAdd(ref char input, ref Password password)
        {
            password.pass.Add(input);
        }
    }

    static public class ConsoleRenderer
    {
        static public short lastExeptionCode = 0;

        static public string passwordStrenghtMessage = "";

        static public void screen(int passLenght, ref int score, ref List<char> pass)
        {
            Console.Clear();
            Console.WriteLine("\t**Please SignIn**\t\n");

            Console.WriteLine(" ID: Mustafa Biçer");

            Console.Write(" password: ");

            passwordStarsRenderer(ref passLenght);

            passStrenghtBarAndMessage(ref passLenght, ref score);

            passErrors(lastExeptionCode);

            doIPrintPasswordToScreen(ref pass);
        }

        static private void passwordStarsRenderer(ref int lenght)
        {
            string stars = new string('*', lenght);
            Console.Write(stars);
        }


        static private void passStrenghtBarAndMessage(ref int lenght, ref int score)
        {
            if (score > 100) { score = 100; }
            string barPositive = new string('+', score / 5);
            string barNegative = new string('-', 20 - score / 5);
            Console.WriteLine("\n\n[{2}{3}] {0}\n-{1} ({4})", lenght, passwordStrenghtMessage, barPositive, barNegative, score);
        }

        static private void passErrors(short exeptionNum)
        {

            switch (exeptionNum)
            {
                case 0:
                    Console.WriteLine("\n-Press esc to exit");
                    break;
                case 1:
                    Console.WriteLine("\n-The password couldn't contain space");
                    break;
                case 2:
                    Console.WriteLine("\n-The password must have at least :\n 2 numbers,2 lower, 2 upper, 2 symbol.");
                    break;
                case 3:
                    Console.WriteLine("\n-The password accepted");
                    break;
                default:
                    break;
            }
        }

        static private void doIPrintPasswordToScreen(ref List<char> pass)
        {
            if (lastExeptionCode == 3)
            {
                foreach (char item in pass)
                {
                    Console.Write(item);
                }
                Console.Write("\n");
                lastExeptionCode = 0;
            }
        }
    }
}
