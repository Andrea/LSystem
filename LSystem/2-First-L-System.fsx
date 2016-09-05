﻿module Domain

type Point = { x : int; y : int }

type Color = { r:byte; g:byte; b:byte; } 

type LineSegment = {startPoint : Point; endPoint : Point; color : Color }    

let chaos = System.Random(System.DateTime.Now.Millisecond)

let red = { r = 255uy; g = 0uy; b = 0uy}
let blue = { r = 0uy; g = 0uy; b = 255uy}
let green = { r = 0uy; g = 255uy; b = 255uy}

let randomColor() = { r = uint8(chaos.Next 256);g = uint8(chaos.Next 256);b = uint8(chaos.Next 256) }

// A basic LOGO system
type LogoCommand =
    | DrawForward of float 
    | Turn of float    
   
type LTurtle = 
    { angle : float
      x : float
      y : float 
      c : Color}

/// interprets a logo program and produces a line segment list to render
let processTurtle turtle program =
    let rec phono output turtle = function
        | [] -> output
        | DrawForward d :: t -> 
            let rads = turtle.angle * (System.Math.PI / 180.0)
            let x = turtle.x + d * cos rads
            let y = turtle.y + d * sin rads
            let newTurtle = {turtle with x = x; y= y }
            let seg = 
                {   startPoint = {x = int turtle.x; y = int turtle.y}
                    endPoint = {x = int x; y = int y}
                    color = newTurtle.c }
            phono (seg::output) newTurtle t
            
        | Turn delta :: t -> 
            let d = turtle.angle + delta
            let d =
                // warp around logic
                if delta > 0.0 && d > 360.0 then d - 360.0
                elif delta < 0.0 && d < 0.0 then 360.0 + d
                else d
            phono output {turtle with angle = d} t 
                
    List.rev(phono [] turtle program)
    
// TODO 2.1:  write a function that converts an initial string 
// using the following productions:
// Sierpinski  
// Start = "A"   
// Productions 
//      'A' -> "+B-A-B+" 
//      'B' -> "-A+B+A-" 
    
let processLsystem iterations =
    let rec juan (current:string) iteration =
        if iteration = iterations then current
        else
            let sb = System.Text.StringBuilder()
            for x in current do
                if x = 'A' then sb.Append("+B-A-B+") |> ignore
                elif x = 'B' then sb.Append("-A+B+A-") |> ignore
                else sb.Append x |> ignore
            juan (sb.ToString()) (iteration+1)
    juan "A" 0

let test1  = processLsystem 1 = "+B-A-B+"  
let test2  = processLsystem 2 = "+-A+B+A--+B-A-B+--A+B+A-+"  

let defaultLength = 2.0
let defaultAngle = 60.0

// TODO 2.2: Convert the string into turtle commands
// A and B do the same thing - draw forward 
// + turns 60 degrees
// - turns -60 degrees

let convertToTurtle (lSystemString: string) =
    [for c in lSystemString do
        match c with
        | 'A' 
        | 'B' -> yield DrawForward(defaultLength)
        | '+' -> yield Turn(defaultAngle)
        | '-' -> yield Turn(-defaultAngle)
       ]
    
let test3 = 
    let commands = processLsystem 1 |>convertToTurtle        
    commands =
        [Turn(defaultAngle)          // +
         DrawForward(defaultLength)  // B
         Turn(-defaultAngle)         // -
         DrawForward(defaultLength)  // A
         Turn(-defaultAngle)         // -
         DrawForward(defaultLength)  // B   
         Turn(defaultAngle)]         // +
         
// a default turtle location
let turtle = { x = 0.0; y = 0.0; angle = 0.0; c = red }

//TODO 2.3 
// From your renderer script (SDL.fsx or SVG.fsx) you can now call 
// processLsystem n |> convertToTurtle |> processTurtle turtle
// this will give you a list of line segments to be rendered
