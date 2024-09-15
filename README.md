# LLaMa Console Application

Questa applicazione console utilizza il modello LLaMa per creare un assistente virtuale che risponde in italiano. Il codice è scritto in C# e utilizza diverse librerie per gestire il caricamento del modello e l'interazione con l'utente.

## Librerie Utilizzate

- **LLama.Common**: Fornisce le classi e i metodi comuni per lavorare con i modelli LLaMa. [Documentazione](https://scisharp.github.io/LLamaSharp/0.5/GetStarted/)
- **LLama**: Contiene le API principali per l'inferenza dei modelli LLaMa e la loro integrazione con le applicazioni C#. [Documentazione](https://scisharp.github.io/LLamaSharp/0.5/GetStarted/)
- **System.Net.Mime.MediaTypeNames**: Utilizzata per gestire i tipi MIME. [Documentazione](https://learn.microsoft.com/en-us/dotnet/api/system.net.mime.mediatypenames?view=net-7.0)
- **System.Reflection**: Utilizzata per ottenere informazioni sull'assembly in esecuzione. [Documentazione](https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=net-7.0)
- **System.Text.RegularExpressions**: Utilizzata per lavorare con le espressioni regolari. [Documentazione](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions?view=net-7.0)

## Installazione

1. Clona questo repository:
    ```bash
    git clone <URL del repository>
    cd <nome del repository>
    ```

2. Assicurati di avere installato .NET 6.0 o superiore.

3. Scarica il modello `llama-2-7b-chat.Q4_K_M.gguf` da Hugging Face:
    - Vai alla pagina del modello su [Hugging Face](https://huggingface.co/).
    - Cerca `llama-2-7b-chat.Q4_K_M.gguf` e scarica il file.

4. Posiziona il file del modello nella directory `LlaModels` all'interno del progetto.

## Esecuzione

1. Apri il progetto in Visual Studio o nel tuo IDE preferito.

2. Modifica il percorso del modello nel file `Program.cs` se necessario:
    ```csharp
    string modelPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../../../../LlaModels/llama-2-7b-chat.Q4_K_M.gguf";
    ```

3. Compila ed esegui l'applicazione:
    ```bash
    dotnet run
    ```

## Utilizzo

L'applicazione inizierà con un prompt che chiede all'utente come sta e se ha domande. L'assistente virtuale risponderà in italiano e continuerà a interagire con l'utente fino a quando non viene digitato "ciao".


## Licenza

Questo progetto è distribuito sotto la licenza AGPL-3.0. Vedi il file `LICENSE` per maggiori dettagli.

