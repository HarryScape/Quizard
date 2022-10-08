"use strict";


// Add Section
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="add-section-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-new-section="modal"]', function (event) {
        SavePosition();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


// Saves question positions
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

        var QuestionJsonHelper = { Id: questionId, SectionId: sectionId, QuestionPosition: count, ParentId: parent, quizId: quizId };
        questionList.push(QuestionJsonHelper);
        count++;
    }

    $.post('/Quiz/SaveQuiz', { updates: questionList },
        function () {
            $('#result').html('successfully called.');
        });
}


// Exit modal methods
$(document).on("click", "#exit-modal", function () {
    $("#exampleModalCenter").modal("toggle");
})

$(document).on("click", "#exit-delete-modal", function () {
    $("#exampleModalCenter").modal("toggle");
})


// Delete Section
function DeleteSection(id) {
    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
        SavePosition();
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
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
                location.reload(true);
            }
        });
    });
}


// Delete Question
function DeleteQuestion(id) {
    SavePosition();
    var quizId = document.getElementById("HiddenQuizId").value;

    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('del-confirm');
    button.addEventListener('click', function handleClick() {
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
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
                location.reload(true);
            }
        });
    });
}


// Delete a quiz
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

// Adds attributes to move objects
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


// Autosave before editing a question
$('a').click(function (event) {
    SavePosition();
});


//Acordion Menu
var acc = document.getElementsByClassName("object");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        // Add and remove active class
        $(".question-edit").click(function (e) {
            e.stopPropagation();
        });

        this.classList.toggle("active");

        // Toggle hide and show panel
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

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-save-question="modal"]', function (event) {
        SavePosition();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            SavePosition();
            placeholder.find('.modal').modal('hide');
            $('#modal-zone').html("");
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

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

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

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

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

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-add-module="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


// Edit Module
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="module-edit-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

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
                location.reload(true);
            });
    });
}


// Enrol Students
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="student-enroll-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-add-students="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post('/Module/EnrollStudents', dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


// Remove Students Modal behaviour
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="student-remove-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })
})


// Remove student from module
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
        complete: function (response) {
            $('#modal-zone').find('.modal').modal('hide');
            $('#modal-zone').html("");
        }
    });
}


// Exit Modal Methods
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


// Begin Quiz
function BeginQuiz() {
    if (time != 0) {
        Countdown();
    } else {
        document.getElementById('timer').innerText = "Unlimited Time";
    }

    var quizId = document.getElementById("HiddenQuizId").value;
    var description = document.getElementById('description');

    $.ajax({
        type: "POST",
        data: { quizId: quizId },
        url: "/TakeQuiz/BeginQuiz",
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay");
            } else {
                console.log("Something went wrong");
            }
        },
        complete: function (response) {
            $('.take-quiz-container').html(response.responseText);
            description.remove();
        }
    });
}


// Checkboxe Behaviour
$(document).on("click", "#check-single", function () {
    var parent = document.getElementById("single").parentElement;
    var checkboxes = parent.querySelectorAll('#check-single');

    $(checkboxes).click(function () {
        $(checkboxes).not(this).prop('checked', false);

    });
});


// Next Section
function NextSection() {
    SubmitAnswers()

    var quizId = document.getElementById("HiddenQuizId").value;
    var element = document.querySelector('.take-quiz-container');
    var index = element.getAttribute('data-index');
    var attemptId = document.getElementById("HiddenAttemptId").value;

    $.ajax({
        type: "POST",
        data: { quizId: quizId, index: index, attemptId: attemptId },
        url: "/TakeQuiz/NextSectionNavigation",
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay");
            } else {
                console.log("Something went wrong");
            }
        },
        complete: function (response) {
            $('.take-quiz-container').html(response.responseText);
            $('.take-quiz-container').attr('data-index', index++);
        }
    });
}


// Previous Section
function PreviousSection() {
    SubmitAnswers()

    var quizId = document.getElementById("HiddenQuizId").value;
    var element = document.querySelector('.take-quiz-container');
    var index = element.getAttribute('data-index');
    var attemptId = document.getElementById("HiddenAttemptId").value;

    $.ajax({
        type: "POST",
        data: { quizId: quizId, index: index, attemptId: attemptId },
        url: "/TakeQuiz/PreviousSectionNavigation",
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay");
            } else {
                console.log("Something went wrong");
            }
        },
        complete: function (response) {
            $('.take-quiz-container').html(response.responseText);
            $('.take-quiz-container').attr('data-index', index--);

        }
    });
}


// Submit Answer
function SubmitAnswers() {
    var attemptId = document.getElementById("HiddenAttemptId").value;
    // Checkbox
    var ansCorrect = [];
    var ansCheck = $('input[type="checkbox"]')
    var ansText = Array.from(document.querySelectorAll('.col-form-label')).map(v => v.innerHTML);
    var questionCheckboxIdList = Array.from(document.querySelectorAll('.col-form-label')).map(v => v.getAttribute('value'));
    var answerIdList = Array.from(document.querySelectorAll('.add-ans')).map(v => v.getAttribute('data-ansId'));

    for (var i = 0; ansCheck[i]; ++i) {
        if (ansCheck[i].checked) {
            ansCorrect.push('true');
        }
        else {
            ansCorrect.push('false');
        }
    }

    // Text
    var questionTextIdList = Array.from(document.querySelectorAll('.form-control')).map(v => v.getAttribute('question'));
    var textResponseList = Array.from(document.querySelectorAll('.form-control')).map(v => v.value);

    $.ajax({
        type: "POST",
        data: { questionTextIdList: questionTextIdList, textResponseList: textResponseList, questionCheckboxIdList: questionCheckboxIdList, ansText: ansText, ansCorrect: ansCorrect, answerIdList: answerIdList, attemptId: attemptId },
        url: "/TakeQuiz/SubmitResponse",
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        success: function (response) {
            if (response != null) {
                console.log("Sent okay");
            } else {
                console.log("Something went wrong");
            }
        }
    });
}


// Complete Quiz
function CompleteQuiz() {
    SubmitAnswers()

    var attemptId = document.getElementById("HiddenAttemptId").value;

    $.post('/TakeQuiz/CompleteQuiz', { attemptId: attemptId },
        function () {
            window.location.href = '/TakeQuiz/Completed/';
        });
}


// Confirm Complete Modal
function ConfirmComplete() {
    $("#exampleModalCenter").modal("toggle");

    const button = document.getElementById('end-confirm');
    button.addEventListener('click', function handleClick() {
        $("#exampleModalCenter").modal("toggle");
        CompleteQuiz();
    });
}


//Add Question New
$(function () {
    var placeholder = $('#modal-zone');

    $('a[data-toggle="section-add-question-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-new-question="modal"]', function (event) {
        SavePosition();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

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
        questionBody[5] = document.getElementById("HiddenSectionId").value; 
        questionBody[6] = document.getElementById("QuestionType").value;

        var data = { questionBody: questionBody, answers: ansText, answersCheck: ansCorrect };

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
            complete: function (response) {
                $('.quiz-wrapper').html(response.responseText);
                placeholder.find('.modal').modal('hide');
                $('#modal-zone').html("");
                location.reload(true);
            }
        });
    });
})


// Add Case Study
$(function () {
    var placeholder = $('#modal-zone');

    $('button[data-toggle="add-casestudy-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholder.html(data);
            placeholder.find('.modal').modal('show');
        })
    })

    placeholder.on('click', '[data-bs-dismiss="modal"]', function (event) {
        placeholder.find('.modal').modal('hide');
        $('#modal-zone').html("");
    })

    placeholder.on('click', '[data-new-casestudy="modal"]', function (event) {
        SavePosition();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataPost = form.serialize();

        $.post(actionUrl, dataPost).done(function (data) {
            location.reload(true);
        })
    });
})


// About Us top button
function ScrollTop() {
    document.body.scrollTop = 0; // Safari
    document.documentElement.scrollTop = 0; // Chrome, Firefox, IE, Edge, Opera
}


// image zoom
$(document).ready(function () {
    $('.case-image').click(function () {
        $(this).toggleClass('case-image-zoom');
    });
});