using LLama.Common;
using LLama;
using System.Reflection;
using LLama.Abstractions;
using LLama.Sampling;


//Come funzionano i Large Language Models LLM , una spiegazione semplice https://www.intelligenzaartificialeitalia.net/post/come-funzionano-i-large-language-models-llm-una-spiegazione-semplice
//Come Usare Llama 3 sul Tuo Computer: L'alternativa Gratuita e OpenSource a GPT-4: https://www.intelligenzaartificialeitalia.net/post/come-usare-llama-3-sul-tuo-computer-l-alternativa-gratuita-e-opensource-a-gpt-4

//LLamaSharp is the C#/.NET binding of llama.cpp. It provides APIs to inference the LLaMa Models and deploy it on native environment or Web.
//It could help C# developers to deploy the LLM (Large Language Model) locally and integrate with C# apps.
//https://scisharp.github.io/LLamaSharp/0.5/GetStarted/
//https://github.com/SciSharp/LLamaSharp

namespace LLaMaConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            bool debug = false;

            //string modelPath = "<Your model path>"; // change it to your own model path
            //string prompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.

            //scegli il modello
            string modelPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../../../../LlaModels/llama-2-7b-chat.Q4_K_M.gguf"; // change it to your own model path
            //string modelPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../../../../LlaModels/gemma-1.1-2b-it.Q3_K_M.gguf"; // change it to your own model path

            //var prompt = "You are Bob. Bob is an Italian teacher. Bob speaks only in Italian and answers general culture questions with precision, in an honest, friendly, proactive manner, trying not to make mistakes. Start by asking the interlocutor how they are and if they have anything to ask.\n"; // use the "chat-with-bob" prompt here.
            var prompt = "Sei Bob: un assistente virtuale che risponde solo in italiano. Devi rispondere a tutte le domande in italiano.";

            // Load model
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024
            };
            using var model = LLamaWeights.LoadFromFile(parameters);

            // Initialize a chat session
            using var context = model.CreateContext(parameters);
            var ex = new InteractiveExecutor(context); //await ex.PrefillPromptAsync(prompt);
            ChatSession session = new ChatSession(ex);

            // show the prompt
            Console.WriteLine();
            Console.Write(prompt);

            //------

            var inferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f
                },
                AntiPrompts = new List<string> { "User:" }
            };

            // Storia della chat
            ChatHistory history = new ChatHistory();
            history.AddMessage(LLama.Common.AuthorRole.User, prompt);

            // run the inference in a loop to chat with LLM
            while (prompt != "ciao")
            {
                string response = "";
                //await foreach (var text in session.ChatAsync(history, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
                await foreach (var text in session.ChatAsync(history, inferenceParams))
                {
                    Console.Write(text); response += text;
                }
                history.AddMessage(LLama.Common.AuthorRole.Assistant, response);

                Console.ForegroundColor = ConsoleColor.Green;
                prompt = Console.ReadLine();
                history.AddMessage(LLama.Common.AuthorRole.User, prompt);
                Console.ForegroundColor = ConsoleColor.White;

                if (debug) { tokenization(model, prompt); embedding(model, context.Params, prompt); }

                //prompt += " Rispondi in italiano. \n";
                //Console.Write(prompt);

            }
        }


        // --- only for test & debug -----

  
        //Tokenization/Detokenization: https://scisharp.github.io/LLamaSharp/0.4/LLamaModel/tokenization/
        static async void tokenization(LLamaWeights model, string text)
        {
            // Tokenizza il testo
            bool add_bos = false; //Allow to add BOS (begin of sentence) and EOS tokens if model is configured to do so.
            bool special = false; //Allow tokenizing special and/ or control tokens which otherwise are not exposed and treated as plaintext. Does not insert a leading space.
            var tokens = model.Tokenize(text, add_bos, special, System.Text.Encoding.Latin1);

            // Stampa i token
            Console.WriteLine("Token:");
            foreach (var token in tokens) Console.WriteLine(token);
            Console.WriteLine("");
        }

        //Get Embeddings: https://scisharp.github.io/LLamaSharp/0.4/LLamaModel/embeddings/
        static async void embedding(LLamaWeights model, IContextParams context, string text)
        {
    
            using var embedder = new LLamaEmbedder(model, context);

            // Ottieni gli embeddings per il testo
            var embeddings = embedder.GetEmbeddings(text);

            // Stampa gli embeddings
            Console.WriteLine("Embeddings:");
            foreach(var embedding in embeddings.Result) Console.WriteLine("[" + string.Join(", ", embedding) + "]");
            Console.WriteLine("");
        }



    }
}

