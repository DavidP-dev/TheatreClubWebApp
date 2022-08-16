module TheatreClubWebApp.Client.Pages.Index

open Feliz
open Feliz.DaisyUI
open Feliz.DaisyUI.Operators


[<ReactComponent>]
let IndexView () =
    Html.div [
        prop.className "flex flex-col gap-4 mx-14"
        prop.children [
            Daisy.alert [
                alert.info
                prop.className "justify-center bg-base-100"
                prop.text "Vítej ve své divadelní databázi. V menu nahoře, klikni, na co potřebuješ."
            ]
            Html.img [
                prop.className "w-30 w-30"
                ++ mask.star2
                prop.src "https://source.unsplash.com/random/400x400/?performance"
            ]
        ]
    ]