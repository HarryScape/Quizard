"use strict";



// This func correctly passes data to controller.
function AddSection() {
    SavePosition();
    var name = document.getElementById("AddSectionName").value;
    var quizId = document.getElementById("HiddenQuizId").value;

    var dataPost = { sectionName: name, quizId: quizId };

    $.ajax({
        type: "POST",
        data: dataPost,
        url: "/Quiz/AddSectionDB",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay", response);
            } else {
                console.log("Something went wrong");
            }
            // var quizUpdate = JSON.stringify(response);
            //// $('.quiz-wrapper').html(quizUpdate);
            // //   $('.quiz-wrapper').html(quizUpdate).html();
            // $('.quiz-wrapper').load(quizUpdate).html();

        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        },
        // Working state
        complete: function (response) {
            $('.quiz-wrapper').html(response.responseText);
        }
    });
}


function AddQuestionGroup() {
    SavePosition();
    var name = document.getElementById("AddGroupName").value;
    var quizId = document.getElementById("HiddenQuizId").value;
    var secId = document.getElementsByClassName("popup")[0].id;

    var dataPost = { groupName: name, quizId: quizId, sectionId: secId };

    console.log(dataPost);

    $.ajax({
        type: "POST",
        data: dataPost,
        url: "/Quiz/AddQuestionGroup",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay", response);
            } else {
                console.log("Something went wrong");
            }
            // var quizUpdate = JSON.stringify(response);
            //// $('.quiz-wrapper').html(quizUpdate);
            // //   $('.quiz-wrapper').html(quizUpdate).html();
            // $('.quiz-wrapper').load(quizUpdate).html();

        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        },
        // Working state
        complete: function (response) {
            $('.quiz-wrapper').html(response.responseText);
        }
    });
}


/*Version 2*/
function SavePosition() {
    var questionList = [];
    var questionDivs = document.querySelectorAll(".object"), i;
    var count = 0;

    for (i = 0; i < questionDivs.length; ++i) {
        var element = questionDivs[i];
        var questionId = element.id;
        var sectionId = element.closest('.popup').id;
        var parent = null;
        if (element.parentNode.closest(".object") !== null) {
            parent = element.parentNode.closest('.object').id;
        }
        //console.log(questionId, parent)

        var QuestionJsonHelper = { Id: questionId, SectionId: sectionId, QuestionPosition: count, ParentId: parent };
        questionList.push(QuestionJsonHelper);
        count++;
    }

    //console.log(questionList);

    $.post('/Quiz/SaveQuiz', { updates: questionList },
        function () {
            $('#result').html('successfully called.');
        });
}



$(document).on("click", "#exit-delete-modal", function () {

    $("#exampleModalCenter").modal("toggle");

})

//$(document).on("click", "#exit-edit-modal", function () {

//    $("#modal-edit").modal("toggle");

//})


function DeleteSection(id) {
    $("#exampleModalCenter").modal("toggle");

    UpdateQuestions();
    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
        console.log('element clicked');
        $("#exampleModalCenter").modal("toggle");

        $.ajax({
            type: "POST",
            data: { sectionId: id },
            url: "/Quiz/DeleteSection",
            contentType: 'application/x-www-form-urlencoded',
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    console.log("Sent okay", response);
                } else {
                    console.log("Something went wrong");
                }
            },
            failure: function (response) {
                console.log(response.responseText);
            },
            error: function (response) {
                console.log(response.responseText);
            },
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
            }
        });
    });
}


function DeleteQuiz(id) {
    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
        $("#exampleModalCenter").modal("toggle");

        $.post('/Quiz/DeleteQuiz', { id: id },
            function (response) {
                window.location.href = response.redirectToUrl;
            });
    });
}



$('#containers .popup, #containers .object ').droppable({
    activeClass: "ui-state-default",
    hoverClass: "ui-state-hover",
    accept: '.object',
    cursor: 'move',
    greedy: true,
    drop: function (event, ui) {
        $(ui.draggable).addClass("insidePopup");
        ui.draggable.detach().appendTo($(this));
    }
});
$('#containers .popup').sortable();
$('#containers .frame').droppable({
    activeClass: "ui-state-default",
    hoverClass: "ui-state-hover",
    accept: '.insidePopup',
    greedy: true,
    drop: function (event, ui) {
        ui.draggable.detach().appendTo($(this));
        $(ui.draggable).removeClass("insidePopup");
    }
});
$('#containers .object').draggable({
    helper: 'clone',
    containment: "document"
});



var acc = document.getElementsByClassName("object");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        /* Toggle between adding and removing the "active" class,
        to highlight the button that controls the panel */
        this.classList.toggle("active");

        /* Toggle between hiding and showing the active panel */
        //var panel = this.nextElementSibling;
        var panel = this.querySelector(".panel");

        if (panel.style.display === "block") {
            panel.style.display = "none";
            event.stopPropagation();
        } else {
            panel.style.display = "block";
            event.stopPropagation();
        }
    });
}

$(function () {
    var placeholder = $('#modal-zone');

    $('a[data-toggle="ajax-edit-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    $(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
        //placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    $(document).on("click", '[data-save="modal"]', function (event) {
        //placeholder.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();
        //var dataUpdate = "";

        $.post(actionUrl, dataPost).done(function (data) {
            SavePosition();
            placeholder.find('.modal').modal('hide');
            $('#modal-zone').html("");
            $('.quiz-wrapper').html(data);
            //dataUpdate = data;
        })
    });
}) 



//function UpdateQuestion() {
//    //$("#exampleModalCenter").modal("toggle");
//    var placeholder = $('#modal-zone');
//    const button = document.getElementById('save-confirm');
//    //button.addEventListener('click', function handleClick() {
//    $(document).on("click", '[data-save="modal"]', function (event) {
//        //$("#exampleModalCenter").modal("toggle");
//        console.log("con")
//        var form = $(this).parents('.modal').find('form').serialize();

//        $.post('/Quiz/UpdateQuestion', form,
//            function (response) {
//                //window.location.href = response.redirectToUrl;
//                placeholder.find('.modal').modal('hide');
//                $('#modal-zone').html("");
//                $('.quiz-wrapper').html(response.responseText);
//            });

//        //$.ajax({
//        //    type: "POST",
//        //    data: form,
//        //    url: "/Quiz/UpdateQuestion",
//        //    dataType: "json",
//        //    success: function (response) {
//        //        if (response != null) {
//        //            console.log("Sent okay");
//        //        } else {
//        //            console.log("Something went wrong");
//        //        }
//        //        // var quizUpdate = JSON.stringify(response);
//        //        //// $('.quiz-wrapper').html(quizUpdate);
//        //        // //   $('.quiz-wrapper').html(quizUpdate).html();
//        //        // $('.quiz-wrapper').load(quizUpdate).html();

//        //    },
//        //    failure: function (response) {
//        //        //console.log(response.responseText);
//        //    },
//        //    error: function (response) {
//        //        //console.log(response.responseText);
//        //    },
//        //    // Working state
//        //    complete: function (response) {
//        //        placeholder.find('.modal').modal('hide');
//        //        $('#modal-zone').html("");
//        //        $('.quiz-wrapper').html(response.responseText);
//        //    }
//        //});
//    });
//}