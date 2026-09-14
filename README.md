![alt text](https://github.com/wrenthewray/wrens-c-sharp-godot-sharepack/blob/main/title-graphic.png "Wren's C# Godot Sharepack Logo")

Hi everyone! This is a work in progress project for a bunch of different components, entities, managers, and plugins I built (or took with credits at the bottom thank you everyone) and like to share between projects. Feel free to use and change as much as you'd like, and if you have any interesting ideas, please feel free to create a PR and offer to merge into this pack! 

Anyone is entitled to use this pack in their games, all I ask is that you add me and anyone else who has worked on this pack to the list of credits for your game. I'll be keeping a running tally of who has worked on this in a separate file, but as of now (July 27th, 2026) I am the only person who has worked on this pack, minus the authors of the plugins. 

Thank you to [Saki](https://store.godotengine.org/asset/saku/takobi-ai/) for their C# behavior trees plugin. Thank you to [Voxy](https://store.godotengine.org/asset/voxy/at-icons/) for one of my favorite plugins with @icons. Thank you to [HolonProduction](https://store.godotengine.org/asset/holonproduction/kanban-tasks/) for the beautiful and simple Kanban Tasks plugin. Finally, thank you to [jembawls](store.godotengine.org/asset/jembawls/controller-icons-c-sharp/) for their port of Controller Icons into C#.

## Goals

This pack sets out to solve one major problem I've personally faced a lot when it comes to game development. You see, what happens often with a project of mine is I'll start with good foundations built on reusable architecture, but then by the time I start wanting to work on actual mechanics and new features, I end up impatiently hacking things together and entangling the good architecture in spaghetti code in an effort to get things done, and then I usually abandon the project when scope creeps out of my grasp and/or burnout occurs. All the work that I did on that project then is lost, nearly impossible to recover without hours of work untangling the mess I made for myself. It just becomes easier to start a new project from scratch, redoing all the implementations of basic core game patterns and wasting time I could be spending actually developing games. 

So, I created this sharepack! Mostly for myself, but I figured other people might be interested in something like this, so I thought to share it on GitHub because I was already hosting this pack on there for myself. 

## Architecture & Design

This pack is built in C# and designed to be used with Godot C# projects. This is because I prefer C#, and I don't see as many people catering to C# or using C# in comparision with GDScript, so that gave me even more reason to tailor this project to C#. I will not be making a GDScript version, but if someone out there wants to, be my guest. 

Everything in this project is stored in the "shared" folder and namespaced appropriately. This makes it easier to share between projects, as you can just copy the folder into your project and go. Namespaces also help a lot with organization, as everything is separated into its own folder and only accessible by explicit reference.