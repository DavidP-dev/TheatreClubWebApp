module TheatreClubWebApp.Client.Pages.AddPerformance

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server


let private alertRow =
    Daisy.alert [
        alert.info
        prop.text "Pro přidání představení vyplň níže zobrazený formulář:"
    ]

let private inputRow =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Název představení:"]
                Daisy.input [input.bordered; prop.placeholder "Název představení"]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Divadlo:"]
                Daisy.input [input.bordered; prop.placeholder "Divadlo"]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Datum a čas představení"]
                Daisy.input [input.bordered; prop.placeholder "Datum a čas představení"]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Počet vstupenek"]
                Daisy.input [input.bordered; prop.placeholder "Počet vstupenek"]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Cena vstupenky"]
                Daisy.input [input.bordered; prop.placeholder "Cena vstupenky"]
            ]

        ]
    ]

let private genresInfo =
    Daisy.alert [
        alert.info
        prop.text "Kliknutím vyber žánry představení:"
        ]

let private genresRow =
    Html.div [
        prop.className "flex flex-row gap-12"
        prop.children [

            // left col
            Html.div [
                prop.className "flex flex-col w-32"
                prop.children [
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Alterna"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Umění"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Komedie"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Tanec"
                        Daisy.checkbox []
                        ]
                    ]
                ]
            ]

            Html.div [
                prop.className "flex flex-col w-32"
                prop.children [
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Drama"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Mejnstrým"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Muzikál"
                        Daisy.checkbox []
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Filosofie"
                        Daisy.checkbox []
                        ]
                    ]
                ]
            ]


        ]
    ]

[<ReactComponent>]

let AddPerformanceView () =
    Html.div [
        prop.className "flex flex-col items-center gap-4 mx-14"
        prop.children [

            alertRow
            inputRow
            genresInfo
            genresRow


            Html.div [

                Daisy.button.button [
                    button.outline
                    button.primary
                    button.lg
                    prop.text "Přidej představení"
                ]
            ]
        ]
    ]
