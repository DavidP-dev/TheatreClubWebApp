module TheatreClubWebApp.Client.Pages.Performances

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server


[<ReactComponent>]
let PerformancesView () =
        let performances, setPerformances = React.useState(List.Empty)
        let loadPerformances () = async {
            let! performances = serviceP.GetPerformances()
            setPerformances performances
        }
        React.useEffectOnce(loadPerformances >> Async.StartImmediate)

        let performanceRows =
            performances
            |> List.map ( fun p ->
                Html.tr [
                    Html.td p.Title
                    Html.td (p.DateAndTime.ToString("dd.MM.YYYY hh:mm"))
                    Html.td p.Tickets
                    Html.td "4"
                    Html.td "Editovat / Smazat"
                ]
                )
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
                        Html.thead [Html.tr [Html.th "Divadelní představení"; Html.th "Datum a čas"; Html.th "Počet vstupenek"; Html.th "Aktivní rezervace"; Html.th "Editace představení";]]
                        Html.tbody performanceRows
                    ]
                ]
            ]
        ]