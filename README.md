# UnityFlow3

Matti
Jeg har lavet på Procedural Content Generation.
Jeg brugt en del a min ferie på at forsøge at få noget til at virke som jeg måtte komme til den konklusion at tutorialen var forældet og at man ikke længere kunne gøre det på den måde.
Jeg brugte så 3 brackeys tutorials som basis for mit procedural som jeg så har addet mit eget spin på.
I mit procedural bruger jeg
1 vertices til at holde fast i punkter som jeg så kan justere højden på med perlin noise.
2 triangles, som skaber meshet.
3 uvs er brugt så mit material ikke ville se mærkeligt ud, da den ikke kan finde ud af hvordan den skal processe det hvis man ikke har defineret det.

Jeg har også en drawGismos() så jeg kan se 

Jeg har også addet 
- Mesh collider Generation, så man rent faktisk ikke bare kan se terrainet men også gå på det.
- Randomizer effekt som når checket randomizer hvor meget perlin noise intensificeres.
- Jeg har også gjort så man bare som værdi kan vælge hvor meget perlin noise intensificeres.

Jeg har lavet min egen ikke baseret på noget specifikt vegetation spawner som spawner random vegetation udfra en liste af valgte prefabs inde for et området, ville gerne havde gjort så hvis det ramte lava at det gik i stykker men kunne ikke lige få mit hovedet rundt om det.

Udover procedural delen af er man en person som skal nå hen til sin ven som står klar med et fly, når man gør det når man hen til mathias bane som starter i luften efter man har hoppet af flyet.
Hvis man rør lavaen er der en UI som fortæller en at man tabte.

Det har været lidt svært for mig at se hvordan jeg skulle lave det bedre herfra, udover det har der været merge fejl selvom at vi arbejdet i hver vores scene. Håber det her kan lade mig gå videre, så jeg kan lave noget bedre næste gang.
