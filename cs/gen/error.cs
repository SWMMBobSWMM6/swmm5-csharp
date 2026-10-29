// Generated from EPA SWMM 5.2.4 (error.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static CPtr ErrString = R.Buf(256);

    // error.c:30
    public static CPtr error_getMsg(int errCode, CPtr msg)
    {
        switch (errCode)
        {
            case 101:
                R.strcpy(msg, S.L46);
                break;
            case 103:
                R.strcpy(msg, S.L47);
                break;
            case 105:
                R.strcpy(msg, S.L48);
                break;
            case 107:
                R.strcpy(msg, S.L49);
                break;
            case 108:
                R.strcpy(msg, S.L50);
                break;
            case 109:
                R.strcpy(msg, S.L51);
                break;
            case 110:
                R.strcpy(msg, S.L52);
                break;
            case 111:
                R.strcpy(msg, S.L53);
                break;
            case 112:
                R.strcpy(msg, S.L54);
                break;
            case 113:
                R.strcpy(msg, S.L55);
                break;
            case 114:
                R.strcpy(msg, S.L56);
                break;
            case 115:
                R.strcpy(msg, S.L57);
                break;
            case 117:
                R.strcpy(msg, S.L58);
                break;
            case 119:
                R.strcpy(msg, S.L59);
                break;
            case 121:
                R.strcpy(msg, S.L60);
                break;
            case 122:
                R.strcpy(msg, S.L61);
                break;
            case 131:
                R.strcpy(msg, S.L62);
                break;
            case 133:
                R.strcpy(msg, S.L63);
                break;
            case 134:
                R.strcpy(msg, S.L64);
                break;
            case 135:
                R.strcpy(msg, S.L65);
                break;
            case 136:
                R.strcpy(msg, S.L66);
                break;
            case 137:
                R.strcpy(msg, S.L67);
                break;
            case 138:
                R.strcpy(msg, S.L68);
                break;
            case 139:
                R.strcpy(msg, S.L69);
                break;
            case 140:
                R.strcpy(msg, S.L70);
                break;
            case 141:
                R.strcpy(msg, S.L71);
                break;
            case 143:
                R.strcpy(msg, S.L72);
                break;
            case 145:
                R.strcpy(msg, S.L73);
                break;
            case 151:
                R.strcpy(msg, S.L74);
                break;
            case 153:
                R.strcpy(msg, S.L75);
                break;
            case 155:
                R.strcpy(msg, S.L76);
                break;
            case 156:
                R.strcpy(msg, S.L77);
                break;
            case 157:
                R.strcpy(msg, S.L78);
                break;
            case 158:
                R.strcpy(msg, S.L79);
                break;
            case 159:
                R.strcpy(msg, S.L80);
                break;
            case 161:
                R.strcpy(msg, S.L81);
                break;
            case 171:
                R.strcpy(msg, S.L82);
                break;
            case 173:
                R.strcpy(msg, S.L83);
                break;
            case 181:
                R.strcpy(msg, S.L84);
                break;
            case 182:
                R.strcpy(msg, S.L85);
                break;
            case 183:
                R.strcpy(msg, S.L86);
                break;
            case 184:
                R.strcpy(msg, S.L87);
                break;
            case 185:
                R.strcpy(msg, S.L88);
                break;
            case 187:
                R.strcpy(msg, S.L89);
                break;
            case 188:
                R.strcpy(msg, S.L90);
                break;
            case 191:
                R.strcpy(msg, S.L91);
                break;
            case 193:
                R.strcpy(msg, S.L92);
                break;
            case 195:
                R.strcpy(msg, S.L93);
                break;
            case 200:
                R.strcpy(msg, S.L94);
                break;
            case 201:
                R.strcpy(msg, S.L95);
                break;
            case 203:
                R.strcpy(msg, S.L96);
                break;
            case 205:
                R.strcpy(msg, S.L97);
                break;
            case 207:
                R.strcpy(msg, S.L98);
                break;
            case 209:
                R.strcpy(msg, S.L99);
                break;
            case 211:
                R.strcpy(msg, S.L100);
                break;
            case 213:
                R.strcpy(msg, S.L101);
                break;
            case 217:
                R.strcpy(msg, S.L102);
                break;
            case 219:
                R.strcpy(msg, S.L103);
                break;
            case 221:
                R.strcpy(msg, S.L104);
                break;
            case 223:
                R.strcpy(msg, S.L105);
                break;
            case 225:
                R.strcpy(msg, S.L106);
                break;
            case 227:
                R.strcpy(msg, S.L107);
                break;
            case 229:
                R.strcpy(msg, S.L108);
                break;
            case 231:
                R.strcpy(msg, S.L109);
                break;
            case 233:
                R.strcpy(msg, S.L110);
                break;
            case 235:
                R.strcpy(msg, S.L111);
                break;
            case 301:
                R.strcpy(msg, S.L112);
                break;
            case 303:
                R.strcpy(msg, S.L113);
                break;
            case 305:
                R.strcpy(msg, S.L114);
                break;
            case 307:
                R.strcpy(msg, S.L115);
                break;
            case 308:
                R.strcpy(msg, S.L116);
                break;
            case 309:
                R.strcpy(msg, S.L117);
                break;
            case 311:
                R.strcpy(msg, S.L118);
                break;
            case 313:
                R.strcpy(msg, S.L119);
                break;
            case 315:
                R.strcpy(msg, S.L120);
                break;
            case 317:
                R.strcpy(msg, S.L121);
                break;
            case 318:
                R.strcpy(msg, S.L122);
                break;
            case 319:
                R.strcpy(msg, S.L123);
                break;
            case 320:
                R.strcpy(msg, S.L124);
                break;
            case 321:
                R.strcpy(msg, S.L125);
                break;
            case 323:
                R.strcpy(msg, S.L126);
                break;
            case 325:
                R.strcpy(msg, S.L127);
                break;
            case 327:
                R.strcpy(msg, S.L128);
                break;
            case 329:
                R.strcpy(msg, S.L129);
                break;
            case 331:
                R.strcpy(msg, S.L130);
                break;
            case 333:
                R.strcpy(msg, S.L131);
                break;
            case 335:
                R.strcpy(msg, S.L132);
                break;
            case 336:
                R.strcpy(msg, S.L133);
                break;
            case 337:
                R.strcpy(msg, S.L134);
                break;
            case 338:
                R.strcpy(msg, S.L135);
                break;
            case 339:
                R.strcpy(msg, S.L136);
                break;
            case 341:
                R.strcpy(msg, S.L137);
                break;
            case 343:
                R.strcpy(msg, S.L138);
                break;
            case 345:
                R.strcpy(msg, S.L139);
                break;
            case 351:
                R.strcpy(msg, S.L140);
                break;
            case 353:
                R.strcpy(msg, S.L141);
                break;
            case 355:
                R.strcpy(msg, S.L142);
                break;
            case 357:
                R.strcpy(msg, S.L143);
                break;
            case 361:
                R.strcpy(msg, S.L144);
                break;
            case 363:
                R.strcpy(msg, S.L145);
                break;
            case 500:
                R.strcpy(msg, S.L146);
                break;
            case 501:
                R.strcpy(msg, S.L147);
                break;
            case 502:
                R.strcpy(msg, S.L148);
                break;
            case 503:
                R.strcpy(msg, S.L149);
                break;
            case 504:
                R.strcpy(msg, S.L150);
                break;
            case 505:
                R.strcpy(msg, S.L151);
                break;
            case 506:
                R.strcpy(msg, S.L152);
                break;
            case 507:
                R.strcpy(msg, S.L153);
                break;
            case 508:
                R.strcpy(msg, S.L154);
                break;
            case 509:
                R.strcpy(msg, S.L155);
                break;
            default:
                R.strcpy(msg, S.L0);
                break;
        }
        return (msg);
    }

    // error.c:45
    public static int error_setInpError(int errcode, CPtr s)
    {
        R.strcpy(S.ErrString, s);
        return errcode;
    }

}
}
