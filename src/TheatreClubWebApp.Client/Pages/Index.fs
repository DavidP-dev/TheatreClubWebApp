module TheatreClubWebApp.Client.Pages.Index

open Feliz

[<ReactComponent>]
let IndexView () =
    Html.div [
        prop.className "flex flex-col items-center h-4/5 w-full justify-center absolute"
        prop.text "Ahoj princezno. V odkazech nahoře najdeš vše, co potřebuješ k vedení klubu"
    ]

