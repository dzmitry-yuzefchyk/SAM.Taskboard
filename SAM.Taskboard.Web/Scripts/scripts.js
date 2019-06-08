

// Settings
let unchangedModel;

function prepareSettings() {
    unchangedModel = $('#profileForm').html();
    console.log(unchangedModel);
}

function undo() {
    $('#profileForm').html(unchangedModel);
}

function openFileDialog() {
    $("#fileInput").click();
}

function loadFile(element) {
    const file = element.files[0];

    if (getFileExtension(file.name) != 'png') {
        return;
    }

    const reader = new FileReader();
    reader.onloadend = function () {
        $('#Icon').attr('value', reader.result);
        $('#icon').attr('src', reader.result);
    }

    reader.readAsDataURL(file);
}

function getFileExtension(fileName) {
    return fileName.split('.').pop();
}
//