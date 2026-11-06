(function () {
  "use strict";

  const autoDismissAlerts = document.querySelectorAll("[data-auto-dismiss='true']");

  autoDismissAlerts.forEach(function (alert) {
    window.setTimeout(function () {
      alert.remove();
    }, 4000);
  });

  const deleteModal = document.getElementById("deleteModal");

  if (deleteModal) {
    deleteModal.addEventListener("show.bs.modal", function (event) {
      const trigger = event.relatedTarget;

      if (!trigger) {
        return;
      }

      const name = trigger.getAttribute("data-delete-name") || "";
      const url = trigger.getAttribute("data-delete-url") || "";
      const warning = trigger.getAttribute("data-delete-warning") || "";

      deleteModal.querySelector("[data-delete-name]").textContent = name;
      deleteModal.querySelector("[data-delete-form]").setAttribute("action", url);

      const warningElement = deleteModal.querySelector("[data-delete-warning]");
      warningElement.textContent = warning;
      warningElement.hidden = warning.length === 0;
    });
  }

  const currentRole = document.getElementById("IsCurrent");
  const endDate = document.getElementById("EndDate");

  if (currentRole && endDate) {
    const syncEndDate = function () {
      if (currentRole.checked) {
        endDate.value = "";
        endDate.disabled = true;
        endDate.removeAttribute("aria-describedby");
      } else {
        endDate.disabled = false;
        endDate.setAttribute("aria-describedby", "EndDate-error");
      }
    };

    currentRole.addEventListener("change", syncEndDate);
    syncEndDate();
  }
})();
