// For more information see https://aka.ms/fsharp-console-apps

open System
open LibGit2Sharp

printfn "Hello from F#"

let folder = "C:\Users\Henning\Desktop\einarbeitung\pdf"// Util.CurrentQueryPath |> Path.GetDirectoryName 

let identity = new Identity("Matthias Henning", "matze_henning@gmx.de")        
let createSignature () = new Signature(identity, DateTimeOffset.Now);

let repo = new Repository(folder)
let commits = repo.Commits.QueryBy("skia/fonts.linq") |> List.ofSeq
