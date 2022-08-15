module TheatreClubWebApp.Client.Pages.EditPerformance

open System
open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open Microsoft.FSharp.Core
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
            DateAndTime = ""
            NumberOfTickets = ""
            NumberOfReservedTickets = "0"
            Cost = ""
            Genres = List.empty<Genre>
        }
        IsValid = false
    }, Cmd.none

let private validate (m:Performance) = true

let update msg (state: Model) =
    match msg with
    | FormChanged m -> { state with Perf = m; IsValid = validate m }, Cmd.none
    | FormSubmitted ->
        let nextCmd =
            if state.IsValid then Cmd.OfAsync.perform serviceP.SavePerformance state.Perf (fun _ -> FormSaved)
            else Cmd.none
        state, nextCmd
    | FormSaved -> state, Page.Performances |> Cmd.navigatePage


let stringDateTimeToDayTimeOffSet (s:string) =
    match s |> DateTimeOffset.TryParse with
    | true, value -> value
    | false, _ -> DateTimeOffset.MinValue

let private alertRow =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Níže změň požadované údaje:"
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
                    prop.defaultValue state.Perf.Title
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
                    prop.defaultValue state.Perf.Theatre
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
                    prop.defaultValue ""
                    prop.onChange (fun v ->
                        { state.Perf with DateAndTime = v  } |> FormChanged |> dispatch
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
                    prop.defaultValue state.Perf.NumberOfTickets
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
                    //prop.type'.number
                    prop.name "Cost"
                    prop.defaultValue state.Perf.Cost
                    prop.onChange (fun v ->
                        { state.Perf with Cost = v} |> FormChanged |> dispatch
                    )
                ]
            ]
        ]
    ]

let private genresInfo =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Klikáním změň žánry představení:"
        ]

let private genresRow state dispatch =
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
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Alternative)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Alternative :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Alternative)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Umění"
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Art)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Art :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Art)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Komedie"
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Comedy)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Comedy :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Comedy)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Tanec"
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Dance)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Dance :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Dance)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
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
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Drama)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Drama :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Drama)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Mejnstrým"
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Mainstream)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Mainstream :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Mainstream)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Muzikál"
                            Daisy.checkbox [
                                prop.isChecked (state.Perf.Genres |> List.contains Genre.Musical)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Musical :: state.Perf.Genres
                                            |> List.distinct
                                        else
                                            state.Perf.Genres
                                            |> List.filter (fun i -> i <> Genre.Musical)
                                    { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Filosofie"
                        Daisy.checkbox [
                            prop.isChecked (state.Perf.Genres |> List.contains Genre.Philosophy)
                            prop.onChange (fun isChecked ->
                                let newValue =
                                    if isChecked then
                                        Genre.Philosophy :: state.Perf.Genres
                                        |> List.distinct
                                    else
                                        state.Perf.Genres
                                        |> List.filter (fun i -> i <> Genre.Philosophy)
                                { state.Perf with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

[<ReactComponent>]

let EditPerformanceView (performanceId:Guid) =
    let state, dispatch = React.useElmish(init, update, [| box performanceId |])

    Html.form [
        prop.onSubmit (fun e ->
                e.preventDefault()
                let performanceId = Guid.NewGuid()
                let msg = "form sent" + performanceId.ToString()
                Fable.Core.JS.console.log(sprintf "%A" state)
                FormSubmitted |> dispatch)

        prop.children [
            Html.div [
                prop.className "flex flex-col items-center gap-4 mx-14"
                prop.children [

                    alertRow
                    inputRow state dispatch
                    genresInfo
                    genresRow state dispatch


                    Html.div [

                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            //prop.disabled (not state.IsValid)
                            prop.text "Ulož změny"
                        ]
                    ]
                ]
            ]
        ]
    ]