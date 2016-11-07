function MM_preloadImages() { //v3.0
    var d=document;
    if(d.images){
        if(!d.MM_p) d.MM_p=new Array();
        var i,j=d.MM_p.length,a=MM_preloadImages.arguments;
        for(i=0; i<a.length; i++)
        if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}
    }
}

function MM_swapImgRestore() { //v3.0
    var i,x,a=document.MM_sr;
    for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_swapImage() { //v3.0
    var i,j=0,x,a=MM_swapImage.arguments;
    document.MM_sr=new Array;
    for(i=0;i<(a.length-2);i+=3){
        if ((x=MM_findObj(a[i]))!=null){
            document.MM_sr[j++]=x;
            if(!x.oSrc) x.oSrc=x.src;
            x.src=a[i+2];
        }
    }
}

function MM_findObj(n, d) { //v4.01
    var p,i,x;
    if(!d) d=document;
    if((p=n.indexOf("?"))>0&&parent.frames.length) {
        d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);
    }
    if(!(x=d[n])&&d.all) x=d.all[n];
    for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
    for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
    if(!x && d.getElementById) x=d.getElementById(n);
    return x;
}

function MM_showHideLayers() { //v6.0
    var i,p,v,obj,args=MM_showHideLayers.arguments;
    for (i=0; i<(args.length-2); i+=3)
        if ((obj=MM_findObj(args[i]))!=null) {
            v=args[i+2];
            if (obj.style) {
                obj=obj.style;
                v=(v=='show')?'visible':(v=='hide')?'hidden':v;
            }
            obj.visibility=v;
            findHideSelect(v);
        }
}

function findHideSelect(v){
    var sobj=document.getElementsByTagName("select");
    if (sobj==null) return;
    for(var i=0;i<sobj.length;++i){
        if (sobj[i].size<=0) sobj[i].style.visibility=(v=='')?'hidden':'visible';
    }
}

function mover(src,color){
    src.style.backgroundColor=color;
}

function mout(src,color){
    src.style.backgroundColor=color;
}

function expandtree(t, i){
    var tree = document.getElementById(t);
    var img = document.getElementById(i);
    if (tree.style.display == '') {
        tree.style.display = 'none';
        img.src = 'images/m_index_12.gif';
    } else {
        tree.style.display = '';
        img.src = 'images/m_index_29.gif';
    }
}

function showDisplayLayers(id){
    var v,top;
    var obj = document.getElementById(new String(id));
    if(obj.style.display == ""){
		v = "none";
        top = 10;
    } else {
        v = "";
        top = null;
    }
    obj.style.display=v;
    obj.style.top = top;
    if (top == null){
        var _oldtop=obj.style.top;
        var ofttop=obj.offsetTop;
        var oftheight=obj.offsetHeight;
        var clientheight=document.body.clientHeight;
        obj.style.top =(oftheight>(clientheight*0.5))?(ofttop>(oftheight*0.5))?(ofttop-(oftheight*0.5)):(((oftheight*0.5)-ofttop)/5):_oldtop;
    }
    findHideSelect(v);
}

