(function () {
  const userId = parseInt(document.body.dataset.userId || "0", 10);
  if (!userId) return; // not logged in — skip notification polling

  const toastEl = document.getElementById("globalNotificationToast");
  const toastBody = document.getElementById("globalNotificationToastBody");
  const toastTime = document.getElementById("globalNotificationTime");
  const ackButton = document.getElementById("ackNotificationBtn");
  const bsToast = new bootstrap.Toast(toastEl, { delay: 5000, autohide: true });

  let currentPopupId = null;
  let isPopupVisible = false;

  async function acknowledgeCurrentPopup() {
    if (!currentPopupId) return;
    const res = await fetch(`/NotificationDisplay/AcknowledgePopup?id=${currentPopupId}&userId=${userId}`, { method: "POST" });
    if (!res.ok) return;
    const data = await res.json();
    if (data.success) currentPopupId = null;
  }

  ackButton.addEventListener("click", async () => {
    await acknowledgeCurrentPopup();
    bsToast.hide();
  });

  async function checkGlobalPopup() {
    try {
      const res = await fetch(`/NotificationDisplay/CheckPopup?userId=${userId}&_=${Date.now()}`);
      if (!res.ok) return;
      const data = await res.json();
      if (data.show && data.id && data.id !== currentPopupId) {
        currentPopupId = data.id;
        toastBody.textContent = data.message;
        toastTime.textContent = "just now";
        bsToast.show();
        isPopupVisible = true;
      }
    } catch (e) {
      console.error("Global popup fetch failed", e);
    }
  }

  setInterval(checkGlobalPopup, 5000);
  checkGlobalPopup();
})();