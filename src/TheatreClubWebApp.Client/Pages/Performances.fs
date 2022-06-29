module TheatreClubWebApp.Client.Pages.Performances

open Feliz
open Feliz.DaisyUI

[<ReactComponent>]
let PerformancesView () =
        Html.div[
            prop.className "flex flex-col gap-4"
            prop.children [
                Html.div[
                    prop.className "flex justify-center"
                    prop.children[
                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Přidej představení"
                            ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th ""; Html.th "Divadelní představení"; Html.th "Datum a čas"; Html.th "Žánr"; Html.th "Aktivní rezervace"; Html.th "Editace představení";]]
                        Html.tbody [Html.tr [Html.td "1"; Html.td "Hubte skauty, serou v lese"; Html.td "10.10.2022 19:30 hod.";  Html.td "Komedie, Taneční"; Html.td "2"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "2"; Html.td "Harold a Maud"; Html.td "09.09.2022 19:00 hod.";  Html.td "Filozofie, Taneční"; Html.td "1"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "3"; Html.td "PROTON!"; Html.td "07.10.2022 19:00 hod."; Html.td "Umění, Filozofie, Komedie"; Html.td "0"; Html.td "Editovat / Smazat"]]
                    ]
                ]
            ]
        ]