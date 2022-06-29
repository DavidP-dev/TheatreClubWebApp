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
                        Html.thead [Html.tr [Html.th ""; Html.th "Příjmení"; Html.th "Jméno"; Html.th "Email"; Html.th "Preferované žánry"; Html.th "Aktivní rezervace"; Html.th "Editace člena"]]
                        Html.tbody [Html.tr [Html.td "1"; Html.td "Dvořáčková"; Html.td "Petra"; Html.td "tloustnurychle@seznam.cz"; Html.td "Komedie, Taneční"; Html.td "2"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "2"; Html.td "Ferjentsik"; Html.td "Karel"; Html.td "karelnahrad@seznam.cz"; Html.td "Filozofie, Taneční"; Html.td "1"; Html.td "Editovat / Smazat"]]
                        Html.tbody [Html.tr [Html.td "3"; Html.td "Pícha"; Html.td "David";Html.td "picha.mda@seznam.cz"; Html.td "Umění, Filozofie, Komedie"; Html.td "0"; Html.td "Editovat / Smazat"]]
                    ]
                ]
            ]
        ]