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

module Transfers =
    let stringToDateTimeOffset (dateTimeString: string) =
        let preParsedString = // Parse to US style format
            dateTimeString[3..4]
            + "/"
            + dateTimeString[0..1]
            + "/"
            + dateTimeString[6..9]
            + " "
            + dateTimeString[11..12]
            + ":"
            + dateTimeString[14..15]

        let parsedTime = DateTimeOffset.TryParse(preParsedString) // Parse to DateTimeOffset

        match parsedTime with // returns correct dateTimeOffset if input is OK
        | true, dTO -> dTO
        | false, _ -> DateTimeOffset.MinValue

type ClientPerformance = // It is modified Performance from domain Performance for better use on webpage
    {
        Id: Guid
        Title: string
        Theatre: string
        DateAndTime: string
        NumberOfAvailableTickets: string
        NumberOfReservedTickets: string
        Cost: string
        Genres: Genre list
    }

module ClientPerformance =
    let fromPerformance (perf: Performance) : ClientPerformance =
        {
            Id = perf.Id
            Title = perf.Title
            Theatre =  perf.Theatre
            DateAndTime = perf.DateAndTime.ToString("dd.MM.yyyy HH:mm")
            NumberOfAvailableTickets = perf.NumberOfAvailableTickets
            NumberOfReservedTickets = perf.NumberOfReservedTickets
            Cost = perf.Cost
            Genres = perf.Genres
        }
    let toPerformance (cPerf: ClientPerformance) : Performance =
        {
            Id = cPerf.Id
            Title = cPerf.Title
            Theatre =  cPerf.Theatre
            DateAndTime = cPerf.DateAndTime |> Transfers.stringToDateTimeOffset
            NumberOfAvailableTickets = cPerf.NumberOfAvailableTickets
            NumberOfReservedTickets = cPerf.NumberOfReservedTickets
            Cost = cPerf.Cost
            Genres = cPerf.Genres
        }

type Model =
    {
        ClientPerformance : ClientPerformance option
        IsValid : bool
    }


type Msg =
   | FormChanged of ClientPerformance
   | LoadPerformance of Guid
   | PerformanceLoaded of Performance
   | FormSubmitted
   | FormSaved

let init (pId: Guid) =
    {
        ClientPerformance = None
        IsValid = false
    }, Cmd.ofMsg (LoadPerformance pId)

let private validate (p:ClientPerformance) =
     String.IsNullOrWhiteSpace(p.Title) |> not
    && String.IsNullOrWhiteSpace(p.Theatre) |> not
    && String.IsNullOrWhiteSpace(p.DateAndTime) |> not
    && String.IsNullOrWhiteSpace(p.NumberOfAvailableTickets) |> not
    && String.IsNullOrWhiteSpace(p.Cost) |> not

let update msg (state: Model) =
    match msg with
    | FormChanged cP -> { state with ClientPerformance = Some cP; IsValid = validate cP }, Cmd.none
    | FormSubmitted ->
        match state.ClientPerformance with
        | Some cP ->
            let isValid = validate cP
            if isValid then
                state, Cmd.OfAsync.perform serviceP.UpdatePerformance (cP |> ClientPerformance.toPerformance) (fun _ -> FormSaved)
            else
                {state with IsValid = isValid}, Cmd.none
        | None -> state, Cmd.none
    | FormSaved -> state, Page.Performances |> Cmd.navigatePage
    | LoadPerformance pId -> state, Cmd.OfAsync.perform serviceP.GetPerformance pId (fun x -> PerformanceLoaded x)
    | PerformanceLoaded p -> { state with ClientPerformance = p |> ClientPerformance.fromPerformance |> Some ; IsValid = true }, Cmd.none

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

let private inputRow (cP: ClientPerformance) dispatch =
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
                    prop.value cP.Title
                    prop.onChange (fun v ->
                        { cP with Title = v } |> FormChanged |> dispatch
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
                    prop.value cP.Theatre
                    prop.onChange (fun v ->
                        { cP with Theatre = v } |> FormChanged |> dispatch
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
                    prop.value cP.DateAndTime
                    prop.onChange (fun v ->
                        { cP with DateAndTime = v } |> FormChanged |> dispatch
                    )
                ]
            ]

            Daisy.formControl [
                Daisy.label [
                    prop.for' "NumberOfTickets"
                    prop.children [
                        Daisy.labelText "Počet dostupných vstupenek:"
                    ]
                ]

                Daisy.input [
                    input.bordered
                    prop.placeholder "Počet dostupných vstupenek"
                    prop.name "NumberOfTickets"
                    prop.value cP.NumberOfAvailableTickets
                    prop.onChange (fun v ->
                        { cP with NumberOfAvailableTickets = v } |> FormChanged |> dispatch
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
                    prop.value cP.Cost
                    prop.onChange (fun v ->
                        { cP with Cost = v} |> FormChanged |> dispatch
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

let private genresRow (cP : ClientPerformance) dispatch =
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
                                prop.isChecked (cP.Genres |> List.contains Genre.Alternative)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Alternative :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Alternative)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Umění"
                            Daisy.checkbox [
                                prop.isChecked (cP.Genres |> List.contains Genre.Art)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Art :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Art)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Komedie"
                            Daisy.checkbox [
                                prop.isChecked (cP.Genres |> List.contains Genre.Comedy)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Comedy :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Comedy)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Tanec"
                            Daisy.checkbox [
                                prop.isChecked (cP.Genres |> List.contains Genre.Dance)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Dance :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Dance)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
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
                                prop.isChecked (cP.Genres |> List.contains Genre.Drama)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Drama :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Drama)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Mejnstrým"
                            Daisy.checkbox [
                                prop.isChecked (cP.Genres |> List.contains Genre.Mainstream)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Mainstream :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Mainstream)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Muzikál"
                            Daisy.checkbox [
                                prop.isChecked (cP.Genres |> List.contains Genre.Musical)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Musical :: cP.Genres
                                            |> List.distinct
                                        else
                                            cP.Genres
                                            |> List.filter (fun i -> i <> Genre.Musical)
                                    { cP with Genres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Filosofie"
                        Daisy.checkbox [
                            prop.isChecked (cP.Genres |> List.contains Genre.Philosophy)
                            prop.onChange (fun isChecked ->
                                let newValue =
                                    if isChecked then
                                        Genre.Philosophy :: cP.Genres
                                        |> List.distinct
                                    else
                                        cP.Genres
                                        |> List.filter (fun i -> i <> Genre.Philosophy)
                                { cP with Genres = newValue } |> FormChanged |> dispatch
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

                    match state.ClientPerformance with
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