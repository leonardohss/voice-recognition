using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace VoiceRecognition
{
    class Program
    {
        private static SpeechRecognitionEngine engine = null;
        private static SpeechSynthesizer sp = null;
        static void Main(string[] args)
        {
            engine = new SpeechRecognitionEngine(new CultureInfo("pt-BR"));
            engine.SetInputToDefaultAudioDevice(); 
            sp = new SpeechSynthesizer();

            //conversas
            string[] conversas = { "Olá", "Boa noite", "Boa tarde", "Tudo Bem" };
            Choices c_conversas = new Choices(conversas);

            GrammarBuilder gb_conversas = new GrammarBuilder();
            gb_conversas.Append(c_conversas);

            Grammar g_conversas = new Grammar(gb_conversas);
            g_conversas.Name = "conversas";

            string[] comandosSistema = {"que horas são", "que dia é hoje" };
            Choices c_comandosSistema = new Choices(comandosSistema);
            GrammarBuilder gb_comandosSistema = new GrammarBuilder();
            gb_comandosSistema.Append(c_comandosSistema);

            Grammar g_comandosSistemas = new Grammar(gb_comandosSistema);
            g_comandosSistemas.Name = "sistema";

            Console.Write("<=============================");
            engine.LoadGrammar(g_comandosSistemas);
            engine.LoadGrammar(g_conversas);
            Console.Write("=============================>");
            engine.SpeechRecognized += rec;
            Console.WriteLine("\nEstou ouvindo...");

            engine.RecognizeAsync(RecognizeMode.Multiple);

            sp.SelectVoiceByHints(VoiceGender.Male);
            Console.ReadKey();

        }

        private static void rec(object s, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Confidence >= 0.4f)
            {
                string speech = e.Result.Text;
                Console.WriteLine("Você disse: " + speech + " Confiança: " + e.Result.Confidence);
                switch (e.Result.Grammar.Name)
                {
                    case "conversas":
                        break;
                    case "sistema":
                        break;
                    default:
                        break;
                } 
            }
            else
            {
                Speak("Não ouvi sua voz claramente, fale novamente...");
            }
        }

        private static void Speak(string text)
        {
            sp.SpeakAsyncCancelAll();
            sp.SpeakAsync(text);
        }
    }
}
