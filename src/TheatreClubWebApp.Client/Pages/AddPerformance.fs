module TheatreClubWebApp.Client.Pages.AddPerformance

open System
open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Shared.Domain

type Model = {
    Perf : Performance
    IsValid : bool
}

type Msg =
   | FormChanged of Performance
   | FormSubmitted
   | FormSaved

let init () =
    {
        Perf = {
            Id = Guid.NewGuid()
            Title = ""
            Theatre = ""
            DateAndTime = DateTimeOffset.Now
            NumberOfTickets = ""
            Reservations = "0"
            Cost = 0
            Genres = List.empty<Genre>
        }
        IsValid = false
    }, Cmd.none


let update msg (state: Model) =
    match msg with
    | FormChanged m -> { state with Perf = m }, Cmd.none
    | FormSubmitted ->

        state, Cmd.OfAsync.perform serviceP.SavePerformance state.Perf (fun _ -> FormSaved)
    | FormSaved -> state, Page.Performances |> Cmd.navigatePage


let private alertRow =
    Daisy.alert [
        alert.info
        prop.text "Pro přidání představení vyplň níže zobrazený formulář:"
    ]

let private inputRow state dispatch =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [
                    prop.for' "Title"
                    prop.children [
                        Daisy.labelText "Název představení:"
                    ]
                ]
                Daisy.input [
                    input.bordered
                    prop.placeholder "Název představení"
                    prop.name "Title"
                    prop.value state.Perf.Title
                    prop.onChange (fun v ->
                        { state.Perf with Title = v } |> FormChanged |> dispatch
                    )
                ]
            ]

            Daisy.formControl [
                Daisy.label [
                    prop.for' "Theatre"
                    prop.children [
                        Daisy.labelText "Divadlo:"
                    ]
                ]

                Daisy.input [
                    input.bordered
                    prop.placeholder "Divadlo"
                    prop.name "Theatre"
                    prop.value state.Perf.Theatre
                    prop.onChange (fun v ->
                        { state.Perf with Theatre = v } |> FormChanged |> dispatch
                    )
                ]
            ]

            Daisy.formControl [
                Daisy.label [
                    prop.for' "DateAndTime"
                    prop.children [
                        Daisy.labelText "Datum a čas představení:"
                    ]
                ]

                Daisy.input [
                    input.bordered
                    prop.placeholder "Datum a čas představení"
                    prop.name "DateAndTime"
                    prop.value state.Perf.DateAndTime
                    prop.onChange (fun v ->
                        { state.Perf with DateAndTime = v } |> FormChanged |> dispatch
                    )
                ]
            ]

            Daisy.formControl [
                Daisy.label [
                    prop.for' "NumberOfTickets"
                    prop.children [
                        Daisy.labelText "Počet vstupenek:"
                    ]
                ]

                Daisy.input [
                    input.bordered
                    prop.placeholder "Počet vstupenek"
                    prop.name "NumberOfTickets"
                    prop.value state.Perf.NumberOfTickets
                    prop.onChange (fun v ->
                        { state.Perf with NumberOfTickets = v } |> FormChanged |> dispatch
                    )
                ]
            ]

            Daisy.formControl [
                Daisy.label [
                    prop.for' "Cost"
                    prop.children [
                        Daisy.labelText "Cena vstupenky:"
                    ]
                ]

                Daisy.input [
                    input.bordered
                    prop.placeholder "Cena stupenky"
                    prop.name "Cost"
                    prop.value state.Perf.Cost
                    prop.onChange (fun v ->
                        { state.Perf with Cost = v } |> FormChanged |> dispatch
                    )
                ]
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
    let state, dispatch = React.useElmish(init, update, [||])

    Html.form [
        Html.div [
            prop.className "flex flex-col items-center gap-4 mx-14"
            prop.children [

                alertRow
                inputRow state dispatch
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
    ]