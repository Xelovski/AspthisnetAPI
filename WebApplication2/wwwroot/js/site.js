function HiD() {
    if (document.getElementById("s111").hidden) { document.getElementById("s111").hidden = false;
    } else { document.getElementById("s111").hidden = true; }}

function ClicToB(a) {
    if (a == 1) {
        document.getElementById("k1").textContent = parseInt(document.getElementById("k1").textContent, 10) + 1;
        if (parseInt(document.getElementById("k1").textContent, 10) > 4) {
            document.getElementById("b1").style.backgroundColor = "blue";
        }
    } else {
        document.getElementById("k2").textContent = parseInt(document.getElementById("k2").textContent, 10) + 1;
        if (parseInt(document.getElementById("k2").textContent, 10) > 4) {
            document.getElementById("b2").style.backgroundColor = "red";
        }
    }
}

function cont() {
    var a = document.getElementById("t1").value;
    if (a.length > 19) { document.getElementById("t1").style.border = "1px solid green"; }
    else { document.getElementById("t1").style.border = "1px solid red"; }
    document.getElementById("am1").textContent = a.length;
}

const osoba = {
    meno: "Jan",
    vek: 21,
    mesto: "Blava"
};
const jsstr = JSON.stringify(osoba);
console.log(jsstr);


const jsdat = '{"meno":"Jan","vek":21,"mesto":"Blava"}';
const obj = JSON.parse(jsdat);
console.log(obj.meno);
console.log(obj.vek);
console.log(obj.mesto);

function giveJ(b) {
    if (b == 1) {
        const ree = {
            meno: "Mart",
            vek: 78,
            mesto: "Express"
        };
        var a = "Meno: ";
        a += ree.meno + ", Vek: " + ree.vek + ", Mesto: " + ree.mesto;
        document.getElementById("out1").textContent = a;
    } else if (b == 2) {
        const osob = {
            meno: "Jan",
            fims: ["supporter", "Frozen", "BadGuy"]
        };
        var f = ""
        osob.fims.forEach(film => f += film + "\n")
        console.log("Oblubene fims of user " + osob.meno + ":\n" + f)
    }
}
console.log("");

const products = [
    {name:"Book", val:150},
    {name:"YOU", val:14},
    {name:"Appartment", val:157000}
]
console.log(products[1].name);
console.log(products[1].val);

console.log("");

const jsst = [
    { meno: "pitr", vek: 78 },
    { meno: "mitr", vek: 12 },
    { meno: "filp", vek: 30 }
]
const q = '[{ "meno": "pitr", "vek": 78 }, { "meno": "mitr", "vek": 12 }, { "meno": "filp", "vek": 30 }]';
console.log(q)
const w = JSON.parse(q)
w.forEach(a => console.log(a.meno + ", " + a.vek))

const zln = '[{"meno":"jan","poz":"domovnik"},{"meno":"pan","poz":"progr"},{"meno":"stfan","poz":"progr"}]';
const zl = JSON.parse(zln)
const prog = zl.filter(zl => zl.poz == "progr");
prog.forEach(f => console.log(f.meno))


function GEN() {
    const knig = [
        { meno: "harripotr", autor: "jkrow" },
        { meno: "panprstov", autor: "jrrtol" },
        { meno: "1948", autor: "orvelG" }
    ]
    const we = JSON.stringify(knig)
    const ou=JSON.parse(we)
    ou.forEach(f => document.getElementById("tabulka").innerHTML +=`<tr class="tr"><td>${f.meno}</td><td>${f.autor}</td></tr>`)
}

function upd() {
    var a = document.getElementById("predmet").value
    if (document.getElementById("predmety").value.length < 1) {
        document.getElementById("q4").textContent += a
        document.getElementById("predmety").value += a
    } else {
        document.getElementById("q4").textContent += ", " + a
        document.getElementById("predmety").value += ", " + a
    }

}



function Del() {
    var a = document.getElementById("sus").value;
    eval(`a = ${a}`);
    var q = [];
    a.forEach(usr => {
        //console.log(a);

        if (document.getElementById(`${usr}`).checked) {
            q.push(usr);
        }
    })
    document.getElementById("here").value = q;
    document.getElementById("suser").style = "display:block;";
}

function CartAdd(a) {
    document.getElementById("Cart01").value+=""+a+",";
}




