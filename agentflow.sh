#!/usr/bin/env bash
# AgentFlow - claude -p で 思考→企画→検証→実践→チェック を回し続ける最小ループ
set -euo pipefail

MAX_CYCLES="${MAX_CYCLES:-10}"
FLOW_DIR=".agentflow"
mkdir -p "$FLOW_DIR"

# 各フェーズに渡す指示。ここを編集すれば挙動が変わる。
prompt() {
  case "$1" in
    think)     echo "${FLOW_DIR}/state.md の課題を読み、今回取り組む1点を選んで理由を ${FLOW_DIR}/01_think.md に書け。" ;;
    plan)      echo "${FLOW_DIR}/01_think.md を読み、実装計画を ${FLOW_DIR}/02_plan.md に書け。" ;;
    verify)    echo "${FLOW_DIR}/02_plan.md を読み、問題点と修正案を ${FLOW_DIR}/03_verify.md に書け。" ;;
    implement) echo "${FLOW_DIR}/02_plan.md と 03_verify.md を踏まえ、C#コードを実装し、変更点を ${FLOW_DIR}/04_implement.md に書け。" ;;
    check)     echo "今回の成果を評価し、${FLOW_DIR}/state.md の課題を更新せよ。総括を ${FLOW_DIR}/05_check.md に書け。" ;;
  esac
}

for ((c=1; c<=MAX_CYCLES; c++)); do
  echo "===== CYCLE $c/$MAX_CYCLES ====="
  for p in think plan verify implement check; do
    echo "--- $p ---"
    claude -p "$(prompt "$p")" \
      --permission-mode acceptEdits \
      --tools "Read,Write,Edit,Glob,Grep" \
      --max-turns 15
  done
done
