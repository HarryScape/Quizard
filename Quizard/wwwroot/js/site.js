"use strict";



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


function SavePosition() {
    var questionList = [];
    var questionDivs = document.querySelectorAll(".object"), i;
    var count = 0;

    for (i = 0; i < questionDivs.length; ++i) {
        var element = questionDivs[i];
        var questionId = element.id;
        var sectionId = element.closest('.popup').id;
        var quizId = document.getElementById("HiddenQuizId").value;
        var parent = null;
        if (element.parentNode.closest(".object") !== null) {
            parent = element.parentNode.closest('.object').id;
        }
        //console.log(questionId, parent)

        var QuestionJsonHelper = { Id: questionId, SectionId: sectionId, QuestionPosition: count, ParentId: parent, quizId: quizId };
        questionList.push(QuestionJsonHelper);
        count++;
    }

    //console.log(questionList);

    $.post('/Quiz/SaveQuiz', { updates: questionList },
        function () {
            $('#result').html('successfully called.');
        });
}



$(document).on("click", "#exit-modal", function () {

    $("#exampleModalCenter").modal("toggle");

})

$(document).on("click", "#exit-delete-modal", function () {

    $("#exampleModalCenter").modal("toggle");

})



function DeleteSection(id) {
    $("#exampleModalCenter").modal("toggle");

    SavePosition();
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
                SavePosition();
                location.reload(true);
            }
        });
    });
}

function DeleteQuestion(id) {
    SavePosition();
    var quizId = document.getElementById("HiddenQuizId").value;

    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
        //console.log('element clicked');
        $("#exampleModalCenter").modal("toggle");

        $.ajax({
            type: "POST",
            data: { questionId: id, quizId: quizId },
            url: "/Quiz/DeleteQuestion",
            contentType: 'application/x-www-form-urlencoded',
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    console.log("Sent okay");
                } else {
                    console.log("Something went wrong");
                }
            },
            failure: function (response) {
                //console.log(response.responseText);
            },
            error: function (response) {
                //console.log(response.responseText);
            },
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
                SavePosition();
                location.reload(true);
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


//Drag and drop
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


//Acordion
var acc = document.getElementsByClassName("object");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        /* Toggle between adding and removing the "active" class,
        to highlight the button that controls the panel */

        $(".question-edit").click(function (e) {
            e.stopPropagation();
        });

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


//Question Options
$(function () {
    var placeholder = $('#modal-zone');

    $('a[data-toggle="ajax-edit-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-save-question="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            SavePosition();
            placeholder.find('.modal').modal('hide');
            $('#modal-zone').html("");
            //$('.quiz-wrapper').html(data);
            SavePosition();
            location.reload(true);
        })
    });
})



//Quiz Options
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="quiz-options-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-save-quiz="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var dataPost = form.serialize();

        $.post('/Dashboard/UpdateQuiz', dataPost).done(function (data) {
            location.reload(true);
        })
    });
})



//Section Options
$(function () {
    var placeholder = $('#modal-zone');

    $('a[data-toggle="section-edit-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-save-section="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            SavePosition();
            placeholder.find('.modal').modal('hide');
            $('#modal-zone').html("");
            //$('.quiz-wrapper').html(data);
            SavePosition();
            location.reload(true);
        })
    });
})


// Add Module
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="add-module-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-add-module="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            location.reload(true);
        })
    });
})




//Edit Module
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="module-edit-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-edit-module="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var dataPost = form.serialize();

        $.post('/Module/EditModule', dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


function DeleteModule(id) {
    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
        $("#exampleModalCenter").modal("toggle");

        $.post('/Module/DeleteModule', { id: id },
            function (response) {
                //window.location.href = response.redirectToUrl;
                location.reload(true);
            });
    });
}


// Enroll Students
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="student-enroll-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-add-students="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post('/Module/EnrollStudents', dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


// Remove Students
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="student-remove-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })
})


function RemoveStudent(studentEmail, moduleId) {
    console.log(studentEmail)
    console.log(moduleId)

    $.ajax({
        type: "POST",
        data: { studentEmail: studentEmail, moduleId: moduleId },
        url: "/Module/RemoveStudents",
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay");
            } else {
                console.log("Something went wrong");
            }
        },
        failure: function (response) {
            //console.log('error 1');
        },
        error: function (response) {
            //console.log('error 2');
        },
        complete: function (response) {
            $('#modal-zone').find('.modal').modal('hide');
            $('#modal-zone').html("");

        }
    });
}

//Add Question
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="add-question-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    //$(document).on("click", '[data-bs-dismiss="modal"]', function (event) {
    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    //$(document).on("click", '[data-save="modal"]', function (event) {
    placeholder.on('click', '[data-new-question="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        //custom
        var ansCorrect = [];
        var ansCheck = $('input[type="checkbox"]')
        var ansText = Array.from(document.querySelectorAll('#ans-txt')).map(v => v.value);
        var ansCheck = $('input[type="checkbox"]')
        for (var i = 0; ansCheck[i]; ++i) {
            if (ansCheck[i].checked) {
                ansCorrect.push('true');
            }
            else {
                ansCorrect.push('false');
            }
        }
        var questionBody = [];
        questionBody[0] = document.getElementById("question-name-text").value;
        questionBody[1] = document.getElementById("inlineFormInputMark").value;
        questionBody[2] = document.getElementById("inlineFormInputMargin").value;
        questionBody[3] = document.getElementById("inlineFormInputNegative").value;
        questionBody[4] = document.getElementById("HiddenQuizId").value;
        questionBody[5] = document.getElementsByClassName("popup")[0].id;
        questionBody[6] = document.getElementById("QuestionType").value;
        //console.log(questionBody);
        //custom

        var data = { questionBody: questionBody, answers: ansText, answersCheck: ansCorrect };

        //$.post('/Quiz/AddQuestion', data).done(function (data) {
        //    //location.reload(true);
        //    $('.quiz-wrapper').html(response.responseText);
        //})
        $.ajax({
            type: "POST",
            data: data,
            url: "/Quiz/AddQuestion",
            contentType: 'application/x-www-form-urlencoded',
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    console.log("Sent okay");
                } else {
                    console.log("Something went wrong");
                }
            },
            failure: function (response) {
                //console.log(response.responseText);
            },
            error: function (response) {
                //console.log(response.responseText);
            },
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
                SavePosition();
                placeholder.find('.modal').modal('hide');
                $('#modal-zone').html("");
                location.reload(true);
            }
        });
    });
})



$(document).on('click', '#add-ans', function (e) {
    e.preventDefault();
    var template = $('#custom-add-box').get(0).outerHTML;
    let count = $("div[id*='custom-add-box']").length;
    if (count < 50) {
        $(".add-ans").append(template).html();
    }
});

$(document).on('click', '#del-ans', function (e) {
    e.preventDefault();
    let count = $("div[id*='custom-add-box']").length;
    if (count > 1) {
        $('.add-ans').children().last().remove();
    }
});



// COUNTDOWN
const countdown = document.getElementById('timer');
const minuteDuration = countdown.getAttribute('time');
let time = minuteDuration * 60;

function Countdown() {
    setInterval(UpdateCountdown, 1000);
}

function UpdateCountdown() {
    const minutes = Math.floor(time / 60);
    let seconds = time % 60;
    var hours = Math.floor(minutes / 60);

    seconds = seconds < 10 ? '0' + seconds : seconds;

    countdown.innerHTML = `${hours} : ${minutes} : ${seconds}`
    time--;

    // if time = zero submit page and load completed page...
}

function BeginQuiz() {
    Countdown();
}