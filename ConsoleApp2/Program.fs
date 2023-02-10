
open System.IO
open LibGit2Sharp
open System

let folder = Directory.GetCurrentDirectory() |> Path.GetDirectoryName 



let identity = new Identity("Matthias Henning", "matze_henning@gmx.de")        
let createSignature () = new Signature(identity, DateTimeOffset.Now);

use repo = new Repository(folder)

let getLastAccess (file: string) = 
    try 
        let head = repo.Commits.QueryBy(file) |> Seq.head
        Some  head.Commit.Committer.When 
    with     
       :? Exception as e  -> None

let addRelative path =   (Path.GetRelativePath(folder, path).Replace(@"\", "/"), path)
    
let notGit (path:string) = path.Contains(".git") |> not

let files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories)
let dirs = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories)

let folderAndDirs = Array.append files dirs  |> Array.where notGit |> Array.map addRelative 

let inner file = 
                  let lastAccess = file |> fst |> getLastAccess 
                  match lastAccess with
                  | None -> None
                  | Some lastAccess -> Some(file, lastAccess)

let lastAccess = folderAndDirs |> Seq.choose inner  

let setDate  ((_, pathAbs : string), date : DateTimeOffset) = match File.Exists pathAbs with
                                                                  | true -> () // File.SetLastWriteTimeUtc(pathAbs, date.UtcDateTime)
                                                                  | false -> ()// Directory.SetLastWriteTimeUtc(pathAbs, date.UtcDateTime)

lastAccess |> Seq.iter setDate
