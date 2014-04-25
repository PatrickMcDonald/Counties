[<EntryPoint>]
let main argv = 
    if argv.Length = 0 then do
        printfn "usage: Counties path"
        System.Environment.Exit 1

    let path = argv.[0]

    if not (System.IO.File.Exists path) then do
        printfn "Could not find %s" path
        System.Environment.Exit 2

    let counties = 
        System.IO.File.ReadAllLines(path)
        |> Array.toList
    
    let decompose (county : string) = 
        county.ToLowerInvariant()
        |> Seq.distinct
        |> Seq.toList
    
    let withLetter counties letter = 
        counties
        |> Seq.where (snd >> Seq.exists ((=) letter))
        |> List.ofSeq
    
    let combineWith f item = (item, f item)
    let decomposedCounties = counties |> List.map (decompose |> combineWith)
    seq { 'a'..'z' }
    |> Seq.map (decomposedCounties
                |> withLetter
                |> combineWith)
    |> Seq.where (snd
                  >> Seq.length
                  >> (=) 1)
    |> Seq.map (fun (letter, counties) -> (letter, fst counties.Head))
    |> Seq.toList
    |> Seq.iter (fun (a, b) -> printfn "%c - %s" a b)
    
    #if DEBUG
    System.Console.ReadKey true |> ignore
    #endif

    0 // return an integer exit code
