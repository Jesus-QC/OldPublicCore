let Subclass = {};
let section = 1;

$(document).load(function (){
    let s = localStorage.getItem("s");
    
    if(s !== null && s !== 1){
        Subclass = JSON.parse(localStorage.getItem("ss"));
        while (section !== s){
            next();
        }
    }
});

function next(){
    onChangingSection(section);
    $("#section-" + section).hide();
    onLoadingSection(section + 1);
    $("#section-" + (section + 1)).show();
    section++;
}

function prev(){
    onChangingSection(section);
    $("#section-" + section).hide();
    onLoadingSection(section - 1);
    $("#section-" + (section - 1)).show();
    section--;
}

function onChangingSection(section){
    switch (section){
        case 1:
            Subclass.Name = $("#name").val();
            Subclass.Description = $("#desc").val();
            $("#prev").show();
            break;
        case 2:
            Subclass.Rarity = $("#rarity").val();
            break;
        case 3:
            Subclass.AffectedRoles = $("#affectedRoles").val();
            break;
        case 4:
            Subclass.SpawnAs = $("#spawnAs").val();
            break;
        case 5:
            Subclass.Team = $("#team").val();
            break;
    }

    localStorage.setItem("s", section);
    localStorage.setItem("ss", JSON.stringify(Subclass));
}

function onLoadingSection(section){
    switch (section){
        case 1:
            $("#prev").hide();
            break;
    }
}