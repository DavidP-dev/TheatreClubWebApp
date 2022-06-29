module TheatreClubWebApp.Client.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

type Page =
    | Index
    | Members
    | Performances
    | Reservations

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Index

    let parseFromUrlSegments = function
        | [ "clenove" ] -> Page.Members
        | [ "predstaveni" ] -> Page.Performances
        | [ "rezervace" ] -> Page.Reservations
        | [ ] -> Page.Index
        | _ -> defaultPage

    let noQueryString segments : string list * (string * string) list = segments, []

    let toUrlSegments = function
        | Page.Index -> [ ] |> noQueryString
        | Page.Members -> [ "clenove" ] |> noQueryString
        | Page.Performances -> [ "predstaveni" ] |> noQueryString
        | Page.Reservations -> [ "rezervace" ] |> noQueryString

[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e:MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigatePath href

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Router.navigatePath

[<RequireQualifiedAccess>]
module Cmd =
    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Cmd.navigatePath