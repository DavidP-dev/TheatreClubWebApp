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
    Perf : Performance option
    IsValid : bool
}

type Msg =
   | FormChanged of Performance
   | LoadPerformance of Guid
   | PerformanceLoaded of Performance
   | FormSubmitted
   | FormSaved

let init (pId: Guid) =
    {
        Perf = None
        IsValid = false
    }, Cmd.ofMsg (LoadPerformance pId)

let private validate (p:Performance) =
     String.IsNullOrWhiteSpace(p.Title) |> not
    && String.IsNullOrWhiteSpace(p.Theatre) |> not
    && String.IsNullOrWhiteSpace(p.DateAndTime) |> not
    && String.IsNullOrWhiteSpace(p.NumberOfTickets) |> not
    && String.IsNullOrWhiteSpace(p.Cost) |> not


let update msg (state: Model) =
    match msg with
    | FormChanged p -> { state with Perf = Some p; IsValid = validate p }, Cmd.none
    | FormSubmitted ->
        let nextCmd =
            match state.Perf with
            | Some p -> Cmd.OfAsync.perform serviceP.UpdatePerformance p (fun _ -> FormSaved)
            | None -> Cmd.none
        state, nextCmd
    | FormSaved -> state, Page.Performances |> Cmd.navigatePage
    | LoadPerformance pId -> state, Cmd.OfAsync.perform serviceP.GetPerformance pId (fun x -> PerformanceLoaded x)
    | PerformanceLoaded p -> { state with Perf = Some p; IsValid = validate p }, Cmd.none

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

let private inputRow (p: Performance) dispatch =
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
                    prop.value p.Title
                    prop.onChange (fun v ->
                        { p with Title = v } |> FormChanged |> dispatch
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
                    prop.value p.Theatre
                    prop.onChange (fun v ->
                        { p with Theatre = v } |> FormChanged |> dispatch
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
                    prop.value p.DateAndTime
                    prop.onChange (fun v ->
                        { p with DateAndTime = v  } |> FormChanged |> dispatch
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
                    prop.value p.NumberOfTickets
                    prop.onChange (fun v ->
                        { p with NumberOfTickets = v } |> FormChanged |> dispatch
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
                    prop.value p.Cost
                    prop.onChange (fun v ->
                        { p with Cost = v} |> FormChanged |> dispatch
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

let private genresRow (p : Performance) dispatch =
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
                                prop.isChecked (p.Genres |> List.contains Genre.Alternative)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Alternative :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Alternative)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Umění"
                            Daisy.checkbox [
                                prop.isChecked (p.Genres |> List.contains Genre.Art)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Art :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Art)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Komedie"
                            Daisy.checkbox [
                                prop.isChecked (p.Genres |> List.contains Genre.Comedy)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Comedy :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Comedy)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Tanec"
                            Daisy.checkbox [
                                prop.isChecked (p.Genres |> List.contains Genre.Dance)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Dance :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Dance)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
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
                                prop.isChecked (p.Genres |> List.contains Genre.Drama)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Drama :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Drama)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Mejnstrým"
                            Daisy.checkbox [
                                prop.isChecked (p.Genres |> List.contains Genre.Mainstream)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Mainstream :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Mainstream)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Muzikál"
                            Daisy.checkbox [
                                prop.isChecked (p.Genres |> List.contains Genre.Musical)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Musical :: p.Genres
                                            |> List.distinct
                                        else
                                            p.Genres
                                            |> List.filter (fun i -> i <> Genre.Musical)
                                    { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Filosofie"
                        Daisy.checkbox [
                            prop.isChecked (p.Genres |> List.contains Genre.Philosophy)
                            prop.onChange (fun isChecked ->
                                let newValue =
                                    if isChecked then
                                        Genre.Philosophy :: p.Genres
                                        |> List.distinct
                                    else
                                        p.Genres
                                        |> List.filter (fun i -> i <> Genre.Philosophy)
                                { p with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

[<ReactComponent>]

let EditPerformanceView (pId:Guid) =

    let state, dispatch = React.useElmish(init pId, update, [| |])

    Html.form [
        prop.onSubmit (fun e ->
                e.preventDefault()
                FormSubmitted |> dispatch
        )
        prop.children [
            Html.div [
                prop.className "flex flex-col items-center gap-4 mx-14"
                prop.children [

                    match state.Perf with
                    | Some p ->
                        alertRow
                        inputRow p dispatch
                        genresInfo
                        genresRow p dispatch


                        Html.div [

                            Daisy.button.button [
                                button.outline
                                button.primary
                                button.lg
                                prop.text "Ulož změny"
                                prop.disabled (not state.IsValid)
                            ]
                        ]
                    | None -> Html.div "Nahrávám..."
                ]
            ]
        ]
    ]