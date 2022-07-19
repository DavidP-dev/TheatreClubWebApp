module TheatreClubWebApp.Client.Pages.AddMember

open System
open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Shared.Domain

type Model = {
    Member : ClubMember
    IsValid : bool
}

type Msg =
   | FormChanged of ClubMember
   | FormSubmitted
   | FormSaved

let init () =
    {
      Member = {
          Id = Guid.NewGuid()
          Name  = ""
          Surname = ""
          Email = ""
          PreferredGenres = List.empty
          MemberReservations = ""
      }
      IsValid = false
    }, Cmd.none

let update msg (state: Model) =
    match msg with
    | FormChanged m -> { state with Member = m }, Cmd.none
    | FormSubmitted ->

        state, Cmd.OfAsync.perform service.SaveClubMember state.Member (fun _ -> FormSaved)
    | FormSaved -> state,Cmd.none

let private alertRow =
    Daisy.alert [
        alert.info
        prop.text "Pro přidání člena vyplň níže zobrazený formulář:"
    ]

let private inputRow state dispatch =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [

                    prop.for' "name"
                    prop.children [
                        Daisy.labelText "Jméno:"
                    ]
                ]
                Daisy.input [
                    input.bordered
                    prop.placeholder "Jméno"
                    prop.name "name"
                    prop.value state.Member.Name
                    prop.onChange (fun v ->
                        { state.Member with Name = v } |> FormChanged |> dispatch
                    )
                ]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Příjmení:"]
                Daisy.input [input.bordered; prop.placeholder "Příjmení"]
            ]
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Email:"]
                Daisy.input [input.bordered; prop.placeholder "Email"]
            ]

        ]
    ]

let private genresInfo =
    Daisy.alert [
        alert.info
        prop.text "Kliknutím vyber preferované žánry:"
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
                            prop.isChecked (state.Member.PreferredGenres |> List.contains Genre.Alternative)
                            prop.onChange (fun isChecked ->
                                let newValue =
                                    if isChecked then
                                        Genre.Alternative :: state.Member.PreferredGenres
                                        |> List.distinct
                                    else
                                        state.Member.PreferredGenres
                                        |> List.filter (fun i -> i <> Genre.Alternative)
                                { state.Member with PreferredGenres = newValue } |> FormChanged |> dispatch
                            )
                        ]
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

let AddMemberView () =

    let state,dispatch = React.useElmish(init, update, [| |])

    Html.form [
            prop.onSubmit (fun e ->
                e.preventDefault()
                let memberId = Guid.NewGuid()
                let msg = "form sent" + memberId.ToString()
                Fable.Core.JS.console.log(sprintf "%A" state)
                FormSubmitted |> dispatch

                )
            prop.children [
                Html.div [
                    prop.className "flex flex-col items-center gap-4 mx-14"
                    prop.children [

                        alertRow
                        inputRow state dispatch
                        genresInfo
                        genresRow state dispatch


                        Html.div [

                            Daisy.button.submit [
                                button.outline
                                button.primary
                                button.lg
                                prop.value "Přidej člena"
                            ]
                        ]
                    ]
                ]
            ]
    ]
