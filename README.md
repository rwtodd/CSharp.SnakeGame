# Snake Game

This is a simple game like the old DOS/arcade games
from long ago.  You are a snake, which grows every
time you eat an apple (`a` on-screen).  All you have
to do is keep eating apples, while not running into
the edge of the board or your own tail.

<pre><code>
+----------------+
|                |
|  a             | Key:
|       @####    |  a    = apple
|           #    |  @### = you
|           ###  |
|                |
+----------------+
</code></pre>

You move with cursor keys.  Press Q to quit.

## Aot Compile
I used this little project as a test of the dotnet AOT compilation
experimental option, and then the official AOT of dotnet 7.0.  For
something this basic, I got a 3Mb binary that ran with 912k of RAM.

Not bad!

