#include <stdio.h>
int main(){
  const char* lines[] = {"HPD37669801HPCPHI19480500010020100000000  2500000000  ",
                         "HPD37669801HPCPHI19480500050101400000005  1500000010  1600000006"};
  char rt[8]={0}, et[8]={0}, rc[8]={0}; long sn=0; int div=0, y=0,m=0,d=0,n=0, hour=0, minute=0; long v=0; char f1=0,f2=0;
  for (int i=0;i<2;i++){
    const char* L=lines[i];
    int r1=sscanf(L, "%3s%6ld%2d%4s", rt, &sn, &div, et);
    int r2=sscanf(L+17, "%4d%2d%4d%3d", &y,&m,&d,&n);
    int r3=sscanf(L+30, "%2d%2d%6ld%c%c", &hour,&minute,&v,&f1,&f2);
    printf("%d [%s] %ld %d [%s] | %d %d %d %d %d | %d %d %d %ld %d %d\n", r1, rt, sn, div, et, r2,y,m,d,n, r3,hour,minute,v,f1,f2);
  }
}
