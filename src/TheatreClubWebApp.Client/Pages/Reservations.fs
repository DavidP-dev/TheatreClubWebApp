module TheatreClubWebApp.Client.Pages.Reservations

open Feliz
open Feliz.DaisyUI

[<ReactComponent>]
let ReservationsView () =
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
                            prop.text "Přidej rezervaci"
                            ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th ""; Html.th "Divadelní přestavení"; Html.th "Datum a čas představení"; Html.th "Objednatel"; Html.th "Zaplaceno"; Html.th "Vstupenky doručeny"; Html.th "Editace představení"]]
                        Html.tbody [Html.tr [Html.td "1"; Html.td "PROTON!"; Html.td "07.10.2022 19:00 hod."; Html.td "Ferjentsik Karel"; Html.td "NE"; Html.td "ANO"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "2"; Html.td "PROTON!"; Html.td "07.10.2022 19:00 hod."; Html.td "Dvořáčková Petra"; Html.td "ANO"; Html.td "NE"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "3"; Html.td "Hubte skauty, serou v lese"; Html.td "10.10.2022 19:00 hod."; Html.td "Pícha David"; Html.td "ANO"; Html.td "ANO"; Html.td "Editovat / Smazat"]]
                    ]
                ]
            ]
        ]