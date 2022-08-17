module TheatreClubWebApp.Client.Pages.EditMember

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Shared.Domain
open Elmish
open Feliz.UseElmish


type Model = {
    Member : ClubMember option
    IsValid : bool
}

type Msg =
   | FormChanged of ClubMember
   | LoadMember of Guid
   | MemberLoaded of ClubMember
   | FormSubmitted
   | FormSaved

let init (cMId: Guid) =
    {
      Member = None
      IsValid = false
    }, Cmd.ofMsg (LoadMember cMId)

let validate (m:ClubMember) =
    String.IsNullOrWhiteSpace(m.Name) |> not
    && String.IsNullOrWhiteSpace(m.Surname) |> not

let update msg (state: Model) =
    match msg with
    | FormChanged m -> { state with Member = Some m; IsValid = validate m }, Cmd.none
    | FormSubmitted ->
        let nextCmd =
            match state.Member with
            | Some m -> Cmd.OfAsync.perform service.UpdateClubMember m (fun _ -> FormSaved)
            | None -> Cmd.none
        state, nextCmd
    | FormSaved -> state, Page.Members |> Cmd.navigatePage
    | LoadMember memberId -> state, Cmd.OfAsync.perform service.GetClubMember memberId (fun x -> MemberLoaded x)
    | MemberLoaded clubMember -> { state with Member = Some clubMember; IsValid = validate clubMember }, Cmd.none

let private alertRow =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Změň požadované údaje:"
    ]

let private inputRow (m:ClubMember) dispatch =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [
                    prop.for' "Name"
                    prop.children [
                        Daisy.labelText "Jméno:"
                    ]
                ]
                Daisy.input [
                    input.bordered
                    prop.placeholder "Jméno"
                    prop.name "Name"
                    prop.value m.Name
                    prop.onChange (fun v ->
                        { m with Name = v } |> FormChanged |> dispatch
                    )
                ]
            ]
            Daisy.formControl [
                Daisy.label [
                    prop.for' "Surname"
                    prop.children [
                        Daisy.labelText "Příjmení:"
                    ]
                ]
                Daisy.input [
                    input.bordered
                    prop.placeholder "Příjmení"
                    prop.name "Surname"
                    prop.value m.Surname
                    prop.onChange (fun v ->
                        {m with Surname = v } |> FormChanged |> dispatch
                    )
                ]
            ]
            Daisy.formControl [
                Daisy.label [
                    prop.for' "Email"
                    prop.children [
                        Daisy.labelText "Email:"
                        ]
                    ]
                Daisy.input [
                        input.bordered
                        prop.placeholder "Email"
                        prop.value m.Email
                        prop.onChange (fun v ->
                            {m with Email = v} |> FormChanged |> dispatch
                        )
                ]
            ]

        ]
    ]

let private genresInfo =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Naklikáním změň preferované žánry:"
        ]

let private genresRow (m:ClubMember) dispatch =
    Html.div [
        prop.className "flex flex-row gap-12"
        prop.children [

            Html.div [
                prop.className "flex flex-col w-32"
                prop.children [
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Alterna"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Alternative)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Alternative :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Alternative)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Umění"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Art)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Art :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Art)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Komedie"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Comedy)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Comedy :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Comedy)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Tanec"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Dance)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Dance :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Dance)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
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
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Drama)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Drama :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Drama)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Mejnstrým"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Mainstream)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Mainstream :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Mainstream)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                            Daisy.labelText "Muzikál"
                            Daisy.checkbox [
                                prop.isChecked (m.PreferredGenres |> List.contains Genre.Musical)
                                prop.onChange (fun isChecked ->
                                    let newValue =
                                        if isChecked then
                                            Genre.Musical :: m.PreferredGenres
                                            |> List.distinct
                                        else
                                            m.PreferredGenres
                                            |> List.filter (fun i -> i <> Genre.Musical)
                                    { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                    Daisy.formControl [
                        Daisy.label [
                        Daisy.labelText "Filosofie"
                        Daisy.checkbox [
                            prop.isChecked (m.PreferredGenres |> List.contains Genre.Philosophy)
                            prop.onChange (fun isChecked ->
                                let newValue =
                                    if isChecked then
                                        Genre.Philosophy :: m.PreferredGenres
                                        |> List.distinct
                                    else
                                        m.PreferredGenres
                                        |> List.filter (fun i -> i <> Genre.Philosophy)
                                { m with PreferredGenres = newValue } |> FormChanged |> dispatch
                                )
                            ]
                        ]
                    ]
                ]
            ]


        ]
    ]

[<ReactComponent>]
let EditMemberView (i:Guid) =

    let state,dispatch = React.useElmish(init i, update, [| |])

    Html.form [
        prop.onSubmit (fun e ->
            e.preventDefault()
            FormSubmitted |> dispatch
        )
        prop.children [
            Html.div [
                prop.className "flex flex-col items-center gap-4 mx-14"
                prop.children [

                    match state.Member with
                    | Some m ->
                        alertRow
                        inputRow m dispatch
                        genresInfo
                        genresRow m dispatch

                        Html.div [

                            Daisy.button.submit [
                                button.outline
                                button.primary
                                button.lg
                                prop.value "Ulož změny"
                                prop.disabled (not state.IsValid)
                            ]
                        ]
                    | None -> Html.div "Náhrávám..."
                ]
            ]
        ]
    ]
