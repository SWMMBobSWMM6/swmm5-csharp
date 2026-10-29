using System; using Swmm5;
public static class T { public static void Main() {
  string[] lines = {"HPD37669801HPCPHI19480500010020100000000  2500000000  ", "HPD37669801HPCPHI19480500050101400000005  1500000010  1600000006"};
  foreach (var s in lines) { var L = R.Lit(s); CPtr rt=R.Buf(8), et=R.Buf(8); long sn=0; int div=0,y=0,m=0,d=0,n=0,hour=0,minute=0,f1=0,f2=0; long v=0;
    int r1=R.sscanf(L, R.Lit("%3s%6ld%2d%4s"), rt, R.RefL(x=>sn=x), R.RefI(x=>div=x), et);
    int r2=R.sscanf(L+17, R.Lit("%4d%2d%4d%3d"), R.RefI(x=>y=x), R.RefI(x=>m=x), R.RefI(x=>d=x), R.RefI(x=>n=x));
    int r3=R.sscanf(L+30, R.Lit("%2d%2d%6ld%c%c"), R.RefI(x=>hour=x), R.RefI(x=>minute=x), R.RefL(x=>v=x), R.RefI(x=>f1=x), R.RefI(x=>f2=x));
    Console.WriteLine($"{r1} [{R.Str(rt)}] {sn} {div} [{R.Str(et)}] | {r2} {y} {m} {d} {n} | {r3} {hour} {minute} {v} {f1} {f2}"); } } }
