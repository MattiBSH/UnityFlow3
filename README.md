# UnityFlow3

Matti


Jeg har lavet Procedural Content Generation.
Jeg brugt en del af min ferie på at forsøge at få noget til at virke som jeg måtte komme til den konklusion at tutorialen var forældet og at man ikke længere kunne gøre det på den måde.
Jeg brugte så 3 brackeys tutorials som basis for mit procedural som jeg så har addet mit eget spin på.
I mit procedural bruger jeg


1 vertices til at holde fast i punkter som jeg så kan justere højden på med perlin noise.


2 triangles, som skaber meshet.


3 uvs er brugt så mit material ikke ville se mærkeligt ud, da den ikke kan finde ud af hvordan den skal processe det hvis man ikke har defineret det.

Jeg har også en drawGismos() så jeg kan se hvor vertices punkterne er.

Jeg har også addet 
- Mesh collider Generation, så man rent faktisk ikke bare kan se terrainet men også gå på det.
- Randomizer effekt som når checket randomizer hvor meget perlin noise intensificeres.
- Jeg har også gjort så man bare som værdi kan vælge hvor meget perlin noise intensificeres.

Jeg har lavet min egen ikke baseret på noget specifikt vegetation spawner som spawner random vegetation udfra en liste af valgte prefabs inde for et området, ville gerne havde gjort så hvis det ramte lava at det gik i stykker men kunne ikke lige få mit hovedet rundt om det.

Udover procedural delen af er man en person som skal nå hen til sin ven som står klar med et fly, når man gør det når man hen til mathias bane som starter i luften efter man har hoppet af flyet.
Hvis man rør lavaen er der en UI som fortæller en at man tabte.

Det har været lidt svært for mig at se hvordan jeg skulle lave det bedre herfra, udover det har der været merge fejl selvom at vi arbejdet i hver vores scene. Håber det her kan lade mig gå videre, så jeg kan lave noget bedre næste gang.


Mathias:
Jeg har valgt at fokusere på PCG så i dette projekt har jeg brugt PCG til at generere en bane.

Banen består af et terrain, som ved hjælp af en Perlin Noise algoritme, kan generere et tilfældigt terrain hver gang banen loades.
Jeg har brugt terrain painter til at tegne textures på terrainet alt efter dets højde og hældning. Så på toppen af bakkerne er der sne, og der hvor bakkerne er stejlest, er der klipper. Resten er en græs texture.
Jeg har dertil også brugt en tree generator til at spawne treer fordelt på det random terrain. Har haft svært ved at få den component til at generere treerne hver gang spillet startes, så man er nødt til at trykke på en knap i den component der genererer dem igen. Buske og højt græs fik jeg aldrig til at virke.

Jeg forsøgte mig med den "Cave generation" tutorial Jesper havde linket til i sine slides, og jeg fulgte den så godt jeg kunne, men noget ville ikke virke, så der sad jeg fast og måtte give op.
