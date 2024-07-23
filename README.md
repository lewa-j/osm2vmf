# [OpenStreetMap](www.openstreetmap.org) to [Valve Map Format](https://developer.valvesoftware.com/wiki/Valve_Map_Format) converter

![Screenshot](res/Screenshot.png)

This is a console program that imports [*.osm XML](https://wiki.openstreetmap.org/wiki/OSM_XML) text file, and exports [vmf](https://developer.valvesoftware.com/wiki/VMF_(Valve_Map_Format)), [obj](https://en.wikipedia.org/wiki/Wavefront_.obj_file) and [map](https://developer.valvesoftware.com/wiki/MAP_(file_format)) .
Currently, it uses information only about buildings.

Since only convex polygons can be used in vmf/map, the program uses the [polygon decomposition algorithm created by Mark Bayazit](https://mpen.ca/406/bayazit).

## How to use it

1) Export osm data from https://www.openstreetmap.org
2) Execute `osm2vmf.exe` with path to file as argument
   > Optional arguments:
   
   > -obj
   
   > -map
   
   > -novmf
4) Open created vmf file in ![image](https://github.com/user-attachments/assets/96d5bbf6-34f0-47c3-8577-c854db78d86c)VHE 4.x or ![image](https://github.com/user-attachments/assets/95d9c550-520d-48b2-8923-0da71804f6fa)Hammer++ 
   (map file in ![image](https://github.com/user-attachments/assets/c3b2e32e-eded-494b-837b-ef144b07eb3b)VHE 3.x, ![image](https://github.com/user-attachments/assets/71d5471b-6cd6-4ef6-a343-0df2e563fed4)JACK or ![image](https://github.com/user-attachments/assets/33ad1dc7-1290-42da-b682-f72f1b63c686)TrenchBroom)

4) Add some entities
5) Compile and run map
