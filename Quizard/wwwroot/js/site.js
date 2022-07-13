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
