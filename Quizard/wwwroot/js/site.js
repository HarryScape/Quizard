"use strict";


// ACCORDION QUESTION

var accordionQuestionButton = document.querySelector(
    "accordion-question-button"
);

document.querySelectorAll(".accordion-question-button").forEach((item) => {
    item.addEventListener("click", (event) => {
        // console.log("click check");
        let accordionQuestionCollapse = item.nextElementSibling;

        if (!item.classList.contains("collapsing")) {
            if (!item.classList.contains("open")) {
                // if has class open
                // remove collapse, add collapsing to class .accordion-collapse (sibling)
                // after x no of time, remove collapsing class and add collapse open class
                // open accordion item
                // console.log("toggle acc button");

                // set height
                accordionQuestionCollapse.style.display = "block";
                let accHeight = accordionQuestionCollapse.clientHeight;

                setTimeout(() => {
                    accordionQuestionCollapse.style.height = accHeight + "px";
                    accordionQuestionCollapse.style.display = "";
                }, 1);

                accordionQuestionCollapse.classList =
                    "accordion-question-collapse collapsing";

                setTimeout(() => {
                    //   console.log("opening");
                    accordionQuestionCollapse.classList =
                        "accordion-question-collapse collapse open";
                    // accCollapse.style.height = "";
                }, 300);
            }
            //close accordion item
            // if doesnt have class open
            // remove class open from .acc-collapse, add collapsing
            // after x no of time remove collapsing and add collapse
            else {
                let accCollapse = item.nextElementSibling;

                accCollapse.classList = "accordion-question-collapse collapsing";

                setTimeout(() => {
                    accCollapse.style.height = "0px";
                }, 1);

                setTimeout(() => {
                    //   console.log("closing");
                    accCollapse.classList = "accordion-question-collapse collapse";
                    accCollapse.style.height = "";
                }, 300);
            }

            item.classList.toggle("open");
        }
    });
});

// DRAG AND DROP QUESTIONS

var draggableQuestions = document.querySelectorAll(
    ".accordion-question-item"
);
var containerSections = document.querySelectorAll(".accordion-section-item");

draggableQuestions.forEach((draggable) => {
    draggable.addEventListener("dragstart", () => {
        draggable.classList.add("dragging");
    });

    draggable.addEventListener("dragend", () => {
        draggable.classList.remove("dragging");
    });
});

containerSections.forEach((container) => {
    container.addEventListener("dragover", (e) => {
        e.preventDefault();
        const afterElement = getDragAfterElement(container, e.clientY);
        const draggable = document.querySelector(".dragging");
        if (afterElement == null) {
            container.appendChild(draggable);
        } else {
            container.insertBefore(draggable, afterElement);
        }
    });
});

function getDragAfterElement(container, y) {
    const draggableElements = [
        ...container.querySelectorAll(".draggable:not(.dragging)"),
    ];

    return draggableElements.reduce(
        (closest, child) => {
            const box = child.getBoundingClientRect();
            const offset = y - box.top - box.height / 2;
            if (offset < 0 && offset > closest.offset) {
                return { offset: offset, element: child };
            } else {
                return closest;
            }
        },
        { offset: Number.NEGATIVE_INFINITY }
    ).element;
}

// DRAG AND DROP SECTION

// const draggableSections = document.querySelectorAll(".accordion-section-item");
// const containerQuiz = document.querySelectorAll(".quiz-wrapper");

// draggableSections.forEach((draggable) => {
//   draggable.addEventListener("dragstart", () => {
//     draggable.classList.add("dragging");
//   });

//   draggable.addEventListener("dragend", () => {
//     draggable.classList.remove("dragging");
//   });
// });

// containerQuiz.forEach((container) => {
//   container.addEventListener("dragover", (e) => {
//     e.preventDefault();
//     const afterElement = getDragAfterElement(container, e.clientY);
//     const draggable = document.querySelector(".dragging");
//     if (afterElement == null) {
//       container.appendChild(draggable);
//     } else {
//       container.insertBefore(draggable, afterElement);
//     }
//   });
// });

// function getDragAfterElement(container, y) {
//   const draggableElements = [
//     ...container.querySelectorAll(".draggable:not(.dragging)"),
//   ];

//   return draggableElements.reduce(
//     (closest, child) => {
//       const box = child.getBoundingClientRect();
//       const offset = y - box.top - box.height / 2;
//       if (offset < 0 && offset > closest.offset) {
//         return { offset: offset, element: child };
//       } else {
//         return closest;
//       }
//     },
//     { offset: Number.NEGATIVE_INFINITY }
//   ).element;
// }



// ADDING SECTIONS.


//function AddSection() {

//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;
//    console.log(name);
//    console.log(quizId);

////    const result = fetch('/Quiz/AddSection', {
////        method: 'POST',
////        body: JSON.stringify({
////            str: name,
////            num: quizId
////        })
////    });
////    const response = await result.json();

////    const result = fetch




//$.ajax({
//    url: '/Quiz/AddSection',
//    data: {
//        'quizId': quizId, 'sectionName': name
//    },
//    type: "POST",
//    success: function () {
//        console.log('woot');
//    }
//});
//}


////fetch(`/Quiz/AddSection`, {
////    method: 'POST',
////    body: JSON.stringify({
////        id: id,
////        one: 'test'
////    })


////var request = new XMLHttpRequest();
////request.open('POST', '/Quiz/AddSection', true);
////request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
////request.send(data);
////}


////(async () => {
////    function AddSection() {
////        var name = document.getElementById("AddSectionName").value;
////        var quizId = document.getElementById("HiddenQuizId").value;

////        const result = await fetch('/Quiz/AddPart', {
////            method: 'POST',
////            body: JSON.stringify({
////                str: name,
////                num: quizId
////            })
////        });
////        const response = await result.json();
////    }
//}


//function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;
//    console.log(name);
//    console.log(quizId);

//    fetch('/QuizController/AddSection', {
//        method: 'POST',
//        body: JSON.stringify({
//            str: 'name',
//            num: 'quizId'
//        })
//    }).then(res => res.json())
//        .then(data => /* do whatever you want with it */ console.log('hell'));
//}




//async function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;

//    const result = await fetch('/Quiz/AddSection', {
//        method: 'POST',
//        body: JSON.stringify({
//            str: name,
//            num: quizId
//        })
//    });
//    const response = await result.json();
//}

// whatever code you need
//dawait AddPart();

//function AddSection() {
//        var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;


//    fetch('/QuizController/AddSection', {
//        method: 'post',
//        body: JSON.stringify({ SectionName: name, quizId: quizId })
//    })
//}

//function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;

//    fetch("/QuizController/AddSection/", {
//        method: "post",
//        headers: {
//            'Accept': 'application/json',
//            'Content-Type': 'application/json'
//        },

//        //make sure to serialize your JSON body
//        body: JSON.stringify({
//            sectionName: name,
//            id: quizId
//        })
//    })
//        .then((response) => {
//            //do something awesome that makes the world a better place
//        });
//}

//function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;
//    console.log('done')
//    $(document).ready(function () {
//        $("#AddSectionBtn").click(function () {
//            var f = {};
//            f.url = '@Url.Action("AddSection", "Quiz")';
//            f.type = "POST";
//            f.dataType = "json";
//            f.data = JSON.stringify({ sectionName: name, quizId: quizId });
//            f.contentType = "application/json";
//            f.success = function (response) {
//                alert("success");
//            };
//            f.error = function (response) {
//                alert("failed");
//            };
//            $.ajax(f);
//        });
//    });
//}


// Fetch API does not work with razor page, ajax it is...
//function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;

//    fetch("/QuizController/AddSection", {

//        credentials: 'include',
//        method: 'POST',
//        headers: {
//            'Accept': 'application/json',
//            'Content-Type': 'application/json'
//            //'X-XSRF-Token': gettoken()
//        },
//        body: JSON.stringify({
//            SectionName: 'test name',
//            QuizId: '2'
//        })
//    }).then(res => {
//        return res.json()
//    })
//        .then(data => console.log(data))
//        .catch(error => console.error('ERROR', error))
//}



//function AddSection() {
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;

//    $.ajax({
//        type: "POST",
//        url: "QuizController/AddSection",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({
//            SectionName: 'test name',
//            QuizId: '2'
//        })
//        error: function (xhr, status, error) {
//        }
//    }).done(function (data) {
//        debugger;
//        $("body").removeClass("loading");

//    });
//}


//function AddSection() {
//    console.log('clicked')
//    var name = document.getElementById("AddSectionName").value;
//    var quizId = document.getElementById("HiddenQuizId").value;

//    $.ajax({
//        type: "POST"
//            url: "@Url.Action("AddSection")",
//        dataType: "json",
//        data: { sectionName: name, quizId: quizId }
//            success: function (result) {
//            console.log(result);
//        },
//        error: function (req, status, error) {
//            console.log(req, status, error)
//        }
//    });
//}


// This func correctly passes data to controller.
function AddSection() {
    UpdateQuestions();
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

function Delete() {
    alert("Sure you want to delete?");
    // Create Delete action in controller.
    // Alert user that questions in this section will be deleted too. 
}


function UpdateQuestions() {
    var questionList = [];
    var questionDivs = document.querySelectorAll(".accordion-question-item"), i;
    var count = 0;

    for (i = 0; i < questionDivs.length; ++i) {
        var element = questionDivs[i];
        var questionId = element.id;
        var sectionId = element.closest('.accordion-section-item').id;
        var QuestionJsonHelper = { Id: questionId, SectionId: sectionId, QuestionPosition: count };
        questionList.push(QuestionJsonHelper);
        count++;
    }

    $.post('/Quiz/SaveQuiz', { updates: questionList },
        function () {
            $('#result').html('"PassThings()" successfully called.');
        });
}



