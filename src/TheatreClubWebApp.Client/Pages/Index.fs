module TheatreClubWebApp.Client.Pages.Index

open Feliz
open Feliz.DaisyUI
open Feliz.DaisyUI.Operators


[<ReactComponent>]
let IndexView () =
    Html.div [
        prop.className "flex flex-col gap-4 mx-auto"
        prop.children [
            Html.div [
                prop.className "flex justify-center"
                prop.children [
                    Html.img [
                        prop.className "h-80 w-80"
                        ++ mask.squircle
                        prop.src "https://source.unsplash.com/random/400x400/?performance"
                    ]
                ]
            ]
            Daisy.alert [
                alert.info
                prop.className "justify-center bg-base-100"
                prop.text "Vítej ve své divadelní databázi. V menu nahoře, klikni, na co potřebuješ."
            ]
        ]
    ]