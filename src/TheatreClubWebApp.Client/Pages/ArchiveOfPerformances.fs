module TheatreClubWebApp.Client.Pages.ArchiveOfPerformances


open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Client.Server


[<ReactComponent>]
let ArchiveOfPerformancesView () =

        let performances, setPerformances = React.useState(List.Empty)

        let loadPerformances () = async {
            let! performances = serviceP.GetPerformances()
            setPerformances performances
        }
        React.useEffectOnce(loadPerformances >> Async.StartImmediate)

        let delete = React.useCallback(fun i ->
            async {
                let! _ = serviceP.DeletePerformance i
                let! _ = loadPerformances ()
                return ()
            }
            |> Async.StartImmediate)

        let performanceRows =
            performances
            |> List.map ( fun p ->
                    Html.tr [
                        Html.td "Dělám na tom"
                        Html.td "Dělám na tom"
                        Html.td "Dělám na tom"
                        Html.td "Dělám na tom"
                        Html.td "Dělám na tom"
                        Html.td "Dělám na tom"
                        Html.td [
                            Daisy.button.button  [
                                prop.className "btn-sm"
                                button.outline
                                button.primary
                                prop.text "Editovat"
                              //  prop.onClick (fun _ -> p.Id |> Page.EditPerformance |> Router.navigatePage)
                            ]
                            Daisy.button.label [
                                prop.htmlFor (p.Id |> string)
                                prop.className "btn-sm"
                                button.outline
                                button.primary
                                prop.text "Smazat"
                            ]
                            Daisy.modalToggle [prop.id (p.Id |> string)]
                            Daisy.modal [
                                prop.children [
                                    Daisy.modalBox [
                                        Html.p $"Opravdu chceš smazat představení s názvem {p.Title}?"
                                        Daisy.modalAction [
                                            Daisy.button.label [
                                                prop.htmlFor (p.Id |> string)
                                                button.primary
                                                prop.text "Ano"
                                //                prop.onClick (fun _ -> delete p.Id)
                                            ]
                                            Daisy.button.label [
                                                prop.htmlFor (p.Id |> string)
                                                button.primary
                                                prop.text "Ne"
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                )
        Html.div[
            prop.className "flex flex-col gap-4"
            prop.children [
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th "Divadelní představení"
                                             Html.th "Divadlo"; Html.th "Datum a čas"
                                             Html.th "Dostupné vstupenky"
                                             Html.th "Rezervované vstupenky"
                                             Html.th "Cena vstupenky"
                                             Html.th "Editace / Smazání představení"]]
                        Html.tbody performanceRows
                    ]
                ]
            ]
        ]