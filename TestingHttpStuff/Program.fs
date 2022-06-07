open System.Net.Http
open System.IO
open System.Threading

let downloadGooglePageAsync = 
    async {
        use client = new HttpClient()
        let! page = client.GetStringAsync("https://www.google.com/search?q=Sanja") |> Async.AwaitTask
        do! File.WriteAllTextAsync("C:/Tmp/newAsyncPage.html", page) |> Async.AwaitTask
        printfn "Google page was downloaded"
    }

[<EntryPoint>]
let main argv =
    Async.Start downloadGooglePageAsync
    while true do 
        printf "%c" '*'
        Thread.Sleep(100)
    0