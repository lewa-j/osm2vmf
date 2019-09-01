# [OpenStreetMap](www.openstreetmap.org) to [Valve Map Format](https://developer.valvesoftware.com/wiki/Valve_Map_Format) converter

This is a console program that imports *.osm text file, and exports vmf and obj.
Currently, it uses information only about buildings.

Since only convex polygons can be used in vmf, the program uses the [polygon decomposition algorithm created by Mark Bayazit](https://bitbucket.org/mnpenner/bayazit-yogesh-poly-decomp/src/default/).

## How to use it

1) Export osm data from https://www.openstreetmap.org
2) Execute `OSM_Parser.exe` with path to file as argument
3) Open created vmf file in VHE
4) Add floor, skybox, some entities
5) Compile and run map
