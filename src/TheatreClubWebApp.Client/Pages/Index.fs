module TheatreClubWebApp.Client.Pages.Index

open Feliz
open Feliz.DaisyUI


[<ReactComponent>]
let IndexView () =
    Html.div [
        prop.className "flex flex-col gap-4 mx-14"
        prop.children
            [
             Daisy.alert [
                alert.info
                prop.className "justify-center"
                prop.text "Vítej ve své divadelní databázi. V menu nahoře, klikni, na co potřebuješ."

        ]
    ]
]